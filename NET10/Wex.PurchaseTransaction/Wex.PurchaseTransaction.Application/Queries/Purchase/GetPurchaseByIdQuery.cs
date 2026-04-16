namespace Wex.PurchaseTransaction.Application.Queries.Purchase
{
    using Cortex.Mediator.Queries;

    public record GetPurchaseByIdQuery(long PurchaseId, string Currency) : IQuery<PurchaseConversionDto>;
}
