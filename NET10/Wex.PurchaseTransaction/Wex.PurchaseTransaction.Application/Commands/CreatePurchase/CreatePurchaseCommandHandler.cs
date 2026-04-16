namespace Wex.PurchaseTransaction.Application.Commands.CreatePurchase
{
    using Cortex.Mediator;
    using Cortex.Mediator.Commands;
    using Microsoft.Extensions.Logging;
    using Wex.PurchaseTransaction.Application.Commands.IdentifiedCommand;
    using Wex.PurchaseTransaction.Domain.AggregatesModel.TransactionAggregate;
    using Wex.PurchaseTransaction.Domain.Idempotency;

    public class CreatePurchaseCommandHandler(IPurchaseRepository purchaseRepository) : ICommandHandler<CreatePurchaseCommand, long>
    {
        public async Task<long> Handle(CreatePurchaseCommand command, CancellationToken cancellationToken)
        {
            var purchase = new Purchase(command.Description, command.TransactionDate, command.PurchaseAmount);
            var purchaseResult = await purchaseRepository.AddAsync(purchase, cancellationToken);
            return purchaseResult.Id;
        }
    }

    // Use for Idempotency in Command process
    public class CreatePurchaseIdentifiedCommandHandler : IdentifiedCommandHandler<CreatePurchaseCommand, long>
    {
        public CreatePurchaseIdentifiedCommandHandler(
            IMediator mediator,
            IRequestManager requestManager,
            ILogger<IdentifiedCommandHandler<CreatePurchaseCommand, long>> logger)
            : base(mediator, requestManager, logger)
        {
        }

        protected override long CreateResultForDuplicateRequest()
        {
            return 0; // Ignore duplicate requests for creating order.
        }
    }
}
