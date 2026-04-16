namespace Wex.PurchaseTransaction.Application.Commands.CreatePurchase
{
    using FluentValidation;

    /// <summary>
    /// Create PurchaseCommandValidator is responsible for validating the CreatePurchaseCommand, 
    /// ensuring that the input data for creating a purchase transaction meets the required criteria. 
    /// It checks that the description is not empty and does not exceed 50 characters, 
    /// that the transaction date is valid and not in the future, and that the purchase amount is greater than zero. 
    /// This validation helps maintain data integrity and prevents invalid purchase transactions from being processed.
    /// </summary>
    public class CreatePurchaseCommandValidator : AbstractValidator<CreatePurchaseCommand>
    {
        public CreatePurchaseCommandValidator()
        {
            RuleFor(x => x.Description)
                .NotEmpty()
                .WithMessage("Description is required")
                .MaximumLength(50)
                .WithMessage("Description cannot exceed 50 characters");

            RuleFor(x => x.TransactionDate)
                .Must(BeAValidDate)
                .WithMessage("Transaction date is not valid")
                .LessThanOrEqualTo(DateTime.UtcNow)
                .WithMessage("Transaction date cannot be in the future");

            RuleFor(x => x.PurchaseAmount)
                .GreaterThan(0)
                .WithMessage("Purchase amount must be greater than zero");
        }

        private bool BeAValidDate(DateTime date)
        {
            return date != default;
        }
    }

}
