namespace Wex.PurchaseTransaction.Domain.AggregatesModel.TransactionAggregate
{
    using Wex.PurchaseTransaction.Domain.SeedWork;

    public class Purchase : Entity, IAggregateRoot
    {
        public string Description { get; private set; } = string.Empty;
        public DateTime TransactionDate { get; private set; }
        public decimal PurchaseAmount { get; private set; }

        public Purchase(string description, DateTime transactionDate, decimal purchaseAmount)
        {
            SetDescription(description);
            SetTransactionDate(transactionDate);
            SetPurchaseAmount(purchaseAmount);
        }

        public void SetDescription(string description)
        {
            if (string.IsNullOrWhiteSpace(description))
                throw new ArgumentException("Description is required.");

            if (description.Length > 50)
                throw new ArgumentException("Description must not exceed 50 characters.");

            Description = description;
        }

        public void SetTransactionDate(DateTime date)
        {
            if (date == default)
                throw new ArgumentException("Transaction date must be a valid date.");

            TransactionDate = date;
        }

        public void SetPurchaseAmount(decimal amount)
        {
            if (amount <= 0)
                throw new ArgumentException("Purchase amount must be positive.");

            PurchaseAmount = Math.Round(amount, 2, MidpointRounding.AwayFromZero);
        }
    }
}
