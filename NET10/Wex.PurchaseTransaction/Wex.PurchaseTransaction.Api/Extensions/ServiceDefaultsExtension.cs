namespace Wex.PurchaseTransaction.Api.Extensions
{
    /// <summary>
    /// Provides extension methods for configuring default services and HTTP client settings in an application builder.
    /// </summary>
    /// <remarks>The extension methods in this class simplify the setup of common service configurations, such
    /// as enabling service discovery and applying default HTTP client resilience policies. These defaults help ensure
    /// that applications are more robust and discoverable in distributed environments.</remarks>
    public static class ServiceDefaultsExtension
    {
        /// <summary>
        /// Configures default services for the application, including service discovery and resilience settings for
        /// HTTP clients.
        /// </summary>
        /// <remarks>This method sets up service discovery and resilience handlers for HTTP clients by
        /// default, ensuring that these features are enabled for all outgoing HTTP requests.</remarks>
        /// <param name="builder">The <see cref="IHostApplicationBuilder"/> instance used to configure the application's services.</param>
        /// <returns>The updated <see cref="IHostApplicationBuilder"/> instance to allow for method chaining.</returns>
        public static IHostApplicationBuilder AddServiceDefaults(this IHostApplicationBuilder builder)
        {
            builder.Services.AddServiceDiscovery();

            builder.Services.ConfigureHttpClientDefaults(http =>
            {
                // Turn on resilience by default
                http.AddStandardResilienceHandler();

                // Turn on service discovery by default
                http.AddServiceDiscovery();
            });

            return builder;
        }
    }
}
