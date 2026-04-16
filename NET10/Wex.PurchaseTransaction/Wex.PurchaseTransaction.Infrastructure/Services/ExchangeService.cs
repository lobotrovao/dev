namespace Wex.PurchaseTransaction.Infrastructure.Services
{
    using System.Net;
    using System.Net.Http.Json;
    using Wex.PurchaseTransaction.Application.Services.Exchange;

    /// <inheritdoc/>
    public class ExchangeService(IHttpClientFactory factory) : IExchangeService
    {
        public async Task<ExchangeDto?> GetExchangeRatesAsync(string date, string currency, string? relativeUrl = null, CancellationToken cancellationToken = default)
        {
            var dateToSearch = DateTime.Parse(date).AddMonths(-6).ToString("yyyy-MM-dd");
            relativeUrl = relativeUrl ?? "?page[number]=1&page[size]=100";
            var parameters = string.Format("{0}&filter=record_date:gte:{1},record_date:lte:{2},country_currency_desc:in:({3})&sort=-record_date", relativeUrl, dateToSearch, date, currency);

            using var client = factory.CreateClient("exchange-api");

            var response = await client.GetAsync(parameters, cancellationToken);

            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<ExchangeDto>(cancellationToken: cancellationToken);
            }

            if (response.StatusCode == HttpStatusCode.NotFound)
            {
                // Handle 404 gracefully
                return null; // or throw custom exception, or fallback logic
            }

            // Handle other errors
            response.EnsureSuccessStatusCode();
            return null;
        }

        public async Task<List<Data>> GetAllExchangeRatesAsync(string date, string currency, CancellationToken cancellationToken = default)
        {
            var allData = new List<Data>();

            string? nextUrl = null;

            do
            {
                var result = await GetExchangeRatesAsync(date, currency, nextUrl, cancellationToken);

                if (result?.Data != null)
                {
                    allData.AddRange(result.Data);
                }

                nextUrl = result?.Links?.Next;

            } while (!string.IsNullOrWhiteSpace(nextUrl));

            return allData;
        }

    }
}
