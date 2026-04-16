namespace Wex.PurchaseTransaction.Application.Queries.Purchase
{
    public record PurchaseDto(long Id, string Description, DateTime TransactionDate, decimal PurchaseAmount);
}
