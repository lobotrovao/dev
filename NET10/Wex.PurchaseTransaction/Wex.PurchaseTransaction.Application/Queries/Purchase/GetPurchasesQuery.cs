namespace Wex.PurchaseTransaction.Application.Queries.Purchase
{
    using Cortex.Mediator.Queries;
    using Wex.PurchaseTransaction.Application.SeedWork;

    public record GetPurchasesQuery(PaginationModel PaginationModel, string SortModel, string FilterModel) : IQuery<PaginationResponse<PurchaseDto>>;        
}
