namespace Wex.PurchaseTransaction.Api.Apis
{ 
    using Cortex.Mediator;
    using Microsoft.AspNetCore.Http.HttpResults;
    using Microsoft.AspNetCore.Mvc;
    using System.Text.Json;
    using Wex.PurchaseTransaction.Application.Commands.CreatePurchase;
    using Wex.PurchaseTransaction.Application.Commands.IdentifiedCommand;
    using Wex.PurchaseTransaction.Application.Queries.Purchase;
    using Wex.PurchaseTransaction.Application.SeedWork;

    /// <summary>
    /// PurchaseApi defines the API endpoints for managing purchases, including retrieving purchase details, 
    /// creating new purchases, and listing purchases with pagination. It uses the Mediator pattern to handle requests and responses, 
    /// ensuring a clean separation of concerns between the API layer and the application logic. The API supports operations such as fetching a purchase by its ID, 
    /// creating a new purchase with necessary details, and retrieving a paginated list of purchases based on specified criteria. 
    /// Each endpoint is designed to return appropriate HTTP status codes and responses based on the outcome of the operations.
    /// </summary>
    public static class PurchaseApi
    {
        /// <summary>
        /// Maps the API endpoints for managing purchases, including retrieval and creation operations.
        /// </summary>
        /// <remarks>This method sets up a route group under the path '/api/purchases' and tags it with
        /// 'Purchases'. It includes endpoints for retrieving all purchases, retrieving a specific purchase by ID, and
        /// creating a new purchase.</remarks>
        /// <param name="app">The <paramref name="app"> instance of <see cref="IEndpointRouteBuilder"/> used to configure the routing for
        /// the purchases API.</param>
        /// <returns>A <see cref="RouteGroupBuilder"/> that represents the configured group of purchase-related API endpoints.</returns>
        public static RouteGroupBuilder MapPurchasesApiV1(this IEndpointRouteBuilder app)
        {
            var group = app.MapGroup("/api/purchases").WithTags("Purchases");
            group.MapGet("/", GetPurchasesAsync);
            group.MapGet("/{purchaseId:long}", GetPurchaseByIdAsync);
            group.MapPost("/", CreatePurchase);
            return group;
        }

        private static async Task<Results<Ok<PurchaseConversionDto>, BadRequest<string>, ProblemHttpResult>> GetPurchaseByIdAsync(long purchaseId, string currency, IMediator mediator)
        {
            var request = new GetPurchaseByIdQuery(purchaseId, currency);
            var query = await mediator.QueryAsync(request);

            if (query == null)
            {
                return TypedResults.Problem($"Purchase with id {purchaseId} not found.", statusCode: StatusCodes.Status404NotFound);
            }

            return TypedResults.Ok(query);
        }

        private static async Task<Results<Ok<long>, BadRequest<string>, ProblemHttpResult>> CreatePurchase(
             [FromHeader(Name = "x-requestid")] Guid requestId,
             CreatePurchaseCommand createPurchaseCommand, [AsParameters] PurchaseServices services)
        {
            services.Logger.LogInformation("Sending command: {CommandName} : {CommandId}", createPurchaseCommand.GetType().Name, requestId);

            if (requestId == Guid.Empty)
            {
                services.Logger.LogWarning("Invalid IntegrationEvent - RequestId is missing - {@IntegrationEvent}", requestId);
                return TypedResults.BadRequest("RequestId is missing.");
            }

            using (services.Logger.BeginScope(new List<KeyValuePair<string, object>> { new("IdentifiedCommandId", requestId) }))
            {
                var command = new IdentifiedCommand<CreatePurchaseCommand, long>(createPurchaseCommand, requestId);
                var resullt = await services.Mediator.SendAsync(command);
                
                if (resullt < 1)
                {
                    services.Logger.LogWarning("CreatePurchaseCommand failed - RequestId: {RequestId}", requestId);
                    return TypedResults.Problem("An error occurred while creating the purchase.", statusCode: StatusCodes.Status500InternalServerError);
                }

                services.Logger.LogInformation("CreatePurchaseCommand succeeded - RequestId: {RequestId}", requestId);
                return TypedResults.Ok(resullt);
            }
        }

        private static async Task<Ok<PaginationResponse<PurchaseDto>>> GetPurchasesAsync(
            IMediator mediator,
            [AsParameters] PaginationRequest request)
        {

            var paginationModel = JsonSerializer.Deserialize<PaginationModel>(request.PaginationModel)
                ?? throw new ArgumentException("Invalid pagination model");

            var query = await mediator.QueryAsync(new GetPurchasesQuery(paginationModel, request.SortModel, request.FilterModel));
            return TypedResults.Ok(query);
        }
    }
}
