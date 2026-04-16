namespace Wex.PurchaseTransaction.Application.Queries.Purchase
{
    using Cortex.Mediator.Queries;
    using Wex.PurchaseTransaction.Application.Services.Exchange;
    using Wex.PurchaseTransaction.Domain.AggregatesModel.TransactionAggregate;

    public class GetPurchaseByIdQueryHandler(IPurchaseRepository purchaseRepository, IExchangeService exchangeService) : IQueryHandler<GetPurchaseByIdQuery, PurchaseConversionDto>
    {
        public async Task<PurchaseConversionDto> Handle(GetPurchaseByIdQuery query, CancellationToken cancellationToken)
        {
            var purchase = await purchaseRepository.GetByIdAsync(query.PurchaseId, cancellationToken);
           
            ArgumentNullException.ThrowIfNull(purchase, $"Purchase with id {query.PurchaseId} not found.");

            var exchangeRate = await exchangeService.GetAllExchangeRatesAsync(purchase.TransactionDate.ToString("yyyy-MM-dd"), query.Currency, cancellationToken);
            var exchangeRateForCurrency = exchangeRate.OrderByDescending(x => x.RecordDate).FirstOrDefault();

            if (exchangeRateForCurrency is null)
            {
                throw new ArgumentException($"Purchage cannot be converted to the target currency {query.Currency}.");
            }
            

            var convertedAmount = Math.Round(purchase.PurchaseAmount * exchangeRateForCurrency.ExchangeRate, 2, MidpointRounding.AwayFromZero);
            DateOnly recordDate = DateOnly.FromDateTime(exchangeRateForCurrency.RecordDate!.Value);
            var exchangeRateDto = new ExchangeRateDto(exchangeRateForCurrency.CountryCurrencyDesc!,exchangeRateForCurrency.ExchangeRate, convertedAmount, recordDate);

            return new PurchaseConversionDto(purchase.Id, purchase.Description, purchase.TransactionDate, purchase.PurchaseAmount, exchangeRateDto);
        }
    }
}
