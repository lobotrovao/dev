namespace Wex.PurchaseTransaction.Application.Commands.CreatePurchase
{
    using Cortex.Mediator.Commands;

    public record CreatePurchaseCommand(string Description, DateTime TransactionDate, decimal PurchaseAmount) : ICommand<long>;
}
