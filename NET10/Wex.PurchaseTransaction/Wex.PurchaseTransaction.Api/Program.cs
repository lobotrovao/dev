namespace Wex.PurchaseTransaction.Api
{
    using Scalar.AspNetCore;
    using Wex.PurchaseTransaction.Api.Apis;
    using Wex.PurchaseTransaction.Api.Extensions;
    using Wex.PurchaseTransaction.Api.Middleware;

    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.AddServiceDefaults();
            builder.AddApplicationServices();

            var withApiVersioning = builder.Services.AddApiVersioning();

            builder.AddDefaultOpenApi(withApiVersioning);

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.MapOpenApi();
                app.MapScalarApiReference();
            }

            app.UseHttpsRedirection();

            app.UseCors();

            var products = app.NewVersionedApi("Products");

            products.MapPurchasesApiV1();

            app.UseDefaultOpenApi();
            app.UseMiddleware<ExceptionHandler>();

            app.Run();
        }
    }
}
