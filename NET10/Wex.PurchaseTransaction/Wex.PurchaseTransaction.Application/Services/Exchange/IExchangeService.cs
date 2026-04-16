namespace Wex.PurchaseTransaction.Application.Services.Exchange
{
    /// <summary>
    /// Defines a contract for retrieving exchange rate data asynchronously for a specified date.
    /// </summary>
    /// <remarks>Implementations of this interface allow callers to obtain exchange rate information in a
    /// flexible format, as determined by the generic type parameter. This enables integration with various data models
    /// or DTOs as required by different consumers.</remarks>
    public interface IExchangeService
    {
        Task<ExchangeDto?> GetExchangeRatesAsync(string date, string currency, string? relativeUrl = null, CancellationToken cancellationToken = default);
        Task<List<Data>> GetAllExchangeRatesAsync(string date, string currency, CancellationToken cancellationToken = default);
    }
}
