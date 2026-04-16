namespace Wex.PurchaseTransaction.Api.Extensions
{
    using Asp.Versioning;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Routing;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Hosting;
    using Scalar.AspNetCore;

    /// <summary>
    /// Provides extension methods for configuring OpenAPI support in ASP.NET applications.
    /// </summary>
    /// <remarks>These methods allow for the integration of OpenAPI documentation and API versioning into the
    /// application pipeline. The UseDefaultOpenApi method sets up the default OpenAPI endpoints, while the
    /// AddDefaultOpenApi method registers OpenAPI services with optional API versioning support.</remarks>
    public static class OpenApiExtensions
    {
        /// <summary>
        /// Configures the application to use the default OpenAPI features based on the application's configuration
        /// settings.
        /// </summary>
        /// <remarks>If the OpenApi section in the configuration does not exist, the method returns the
        /// application without making any changes. In development mode, it also maps a scalar API reference and sets up
        /// a redirect for the root path.</remarks>
        /// <param name="app">The <see cref="WebApplication"/> instance to which the OpenAPI features will be applied.</param>
        /// <returns>The original <see cref="IApplicationBuilder"/> instance, allowing for method chaining.</returns>
        public static IApplicationBuilder UseDefaultOpenApi(this WebApplication app)
        {
            var configuration = app.Configuration;
            var openApiSection = configuration.GetSection("OpenApi");

            if (!openApiSection.Exists())
            {
                return app;
            }

            app.MapOpenApi();

            if (app.Environment.IsDevelopment())
            {
                app.MapScalarApiReference(options =>
                {
                    // Disable default fonts to avoid download unnecessary fonts
                    options.DefaultFonts = false;
                });
                app.MapGet("/", () => Results.Redirect("/scalar/v1")).ExcludeFromDescription();
            }

            return app;
        }

        /// <summary>
        /// Configures the application to use OpenAPI documentation with default settings, and optionally integrates API
        /// versioning support.
        /// </summary>
        /// <remarks>When API versioning is specified, this method sets up OpenAPI documentation for each
        /// defined API version and formats group names accordingly. This facilitates the generation of separate OpenAPI
        /// documents for each API version, supporting versioned APIs in the application.</remarks>
        /// <param name="builder">The application builder used to configure services and middleware for the application.</param>
        /// <param name="apiVersioning">An optional API versioning builder. If provided, enables API versioning features and configures OpenAPI
        /// documentation for each API version.</param>
        /// <returns>The application builder instance, allowing for further configuration.</returns>
        public static IHostApplicationBuilder AddDefaultOpenApi(
            this IHostApplicationBuilder builder,
            IApiVersioningBuilder? apiVersioning = default)
        {

            if (apiVersioning is not null)
            {
                // the default format will just be ApiVersion.ToString(); for example, 1.0.
                // this will format the version as "'v'major[.minor][-status]"
                var versioned = apiVersioning.AddApiExplorer(options =>
                {
                    options.GroupNameFormat = "'v'VVV";
                    options.SubstituteApiVersionInUrl = true;
                });

                string[] versions = ["v1", "v2"];
                foreach (var description in versions)
                {
                    builder.Services.AddOpenApi(description, options =>
                    {
                        options.AddDocumentTransformer((document, context, cancellationToken) =>
                        {
                            return Task.CompletedTask;
                        });

                    });
                }
            }

            return builder;
        }
    }
}