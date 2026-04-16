namespace Wex.PurchaseTransaction.Api.Extensions
{
    using Cortex.Mediator.Behaviors;
    using Cortex.Mediator.Commands;
    using Cortex.Mediator.DependencyInjection;
    using FluentValidation;
    using Microsoft.EntityFrameworkCore;
    using System.Reflection;
    using Wex.PurchaseTransaction.Application.Commands.CreatePurchase;
    using Wex.PurchaseTransaction.Application.Services.Exchange;
    using Wex.PurchaseTransaction.Domain.AggregatesModel.TransactionAggregate;
    using Wex.PurchaseTransaction.Domain.Idempotency;
    using Wex.PurchaseTransaction.Infrastructure.Databases;
    using Wex.PurchaseTransaction.Infrastructure.Idempotency;
    using Wex.PurchaseTransaction.Infrastructure.Repositories;
    using Wex.PurchaseTransaction.Infrastructure.Services;

    /// <summary>
    /// Provides extension methods for configuring application services in the dependency injection container.
    /// </summary>
    /// <remarks>This class contains methods to register various services, including database contexts,
    /// repositories, HTTP clients, and CORS policies. It is intended to be used in the application startup process to
    /// set up necessary services for the application.</remarks>
    public static class ApplicationServicesExtension
    {
        /// <summary>
        /// Configures essential application services, including database context, dependency injection, HTTP clients,
        /// CORS policies, form options, and logging for the application.
        /// </summary>
        /// <remarks>This method should be called during application startup to ensure that all required
        /// services, such as the database context, repositories, external API clients, and CORS policies, are properly
        /// registered. It also configures form options to support large file uploads and enables logging. Adjust CORS
        /// settings as appropriate for production environments to restrict allowed origins, methods, and
        /// headers.</remarks>
        /// <param name="builder">The application builder used to register and configure services for the host.</param>
        public static void AddApplicationServices(this IHostApplicationBuilder builder)
        {
            var services = builder.Services;
            services.AddDbContext<PurchaseDbContext>(options =>
            {
                ////options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"), op => op.EnableRetryOnFailure());
                options.UseInMemoryDatabase("PurchasesDb");
            });

            services.AddValidatorsFromAssemblyContaining<CreatePurchaseCommandValidator>();

            // Assemblies to scan for handlers
            services.AddCortexMediator(
                [typeof(Application.AssemblyReference), typeof(Infrastructure.AssemblyReference)]
            );
            
            services.AddTransient(typeof(ICommandPipelineBehavior<,>), typeof(ValidationCommandBehavior<,>));
            services.AddScoped<IPurchaseRepository, PurchaseRepository>();
            services.AddScoped<IExchangeService, ExchangeService>();
            services.AddScoped<IRequestManager, RequestManager>();

            // Configure the HttpClient for the exchange API, we will use it to get the exchange rates for the current date,
            // and convert the price of the product to USD, we will use the base address from the configuration,
            // and we will use the name "exchange-api" to create the client in the ExchangeService
            services.AddHttpClient("exchange-api", (serviceProvider, client) =>
            {
                ArgumentException.ThrowIfNullOrEmpty(builder.Configuration["ExchangeApi"], "ExchangeApi configuration is missing.");

                client.BaseAddress = new Uri(builder.Configuration["ExchangeApi"]!);
            });

            // Cors configuration to allow any origin, method, and header (for development purposes), when production use the specific origins, methods, and headers
            services.AddCors(options =>
            {
                options.AddDefaultPolicy(policy =>
                {
                    policy.AllowAnyOrigin()
                          .AllowAnyMethod()
                          .AllowAnyHeader();
                });
            });

            services.AddLogging();
        }
    }
}
