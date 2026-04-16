namespace Wex.PurchaseTransaction.Application.Queries.Purchase
{
    public record PurchaseConversionDto(long Id, string Description, DateTime TransactionDate, decimal PurchaseAmount, ExchangeRateDto? ExchangeRate);
    public record ExchangeRateDto(string Currency, decimal Rate, decimal ConvertedAmount, DateOnly ExchangeRateDate);
}
