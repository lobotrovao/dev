namespace Wex.PurchaseTransaction.Tests
{
    using Wex.PurchaseTransaction.Application.Commands.CreatePurchase;

    public class CreatePurchaseCommandValidatorTests
    {
        [Fact]
        public void Validator_ValidCommand_PassesValidation()
        {
            // Arrange
            var command = new CreatePurchaseCommand("valid desc", DateTime.UtcNow.AddDays(-1), 10m);
            var validator = new CreatePurchaseCommandValidator();

            // Act
            var result = validator.Validate(command);

            // Assert
            Assert.True(result.IsValid);
        }

        [Fact]
        public void Validator_EmptyDescription_FailsValidation()
        {
            var command = new CreatePurchaseCommand(string.Empty, DateTime.UtcNow.AddDays(-1), 10m);
            var validator = new CreatePurchaseCommandValidator();

            var result = validator.Validate(command);

            Assert.False(result.IsValid);
            Assert.Contains(result.Errors, e => e.ErrorMessage.Contains("Description is required") || e.PropertyName == "Description");
        }

        [Fact]
        public void Validator_FutureDate_FailsValidation()
        {
            var command = new CreatePurchaseCommand("desc", DateTime.UtcNow.AddDays(1), 10m);
            var validator = new CreatePurchaseCommandValidator();

            var result = validator.Validate(command);

            Assert.False(result.IsValid);
            Assert.Contains(result.Errors, e => e.ErrorMessage.Contains("Transaction date cannot be in the future") || e.PropertyName == "TransactionDate");
        }

        [Fact]
        public void Validator_NonPositiveAmount_FailsValidation()
        {
            var command = new CreatePurchaseCommand("desc", DateTime.UtcNow.AddDays(-1), 0m);
            var validator = new CreatePurchaseCommandValidator();

            var result = validator.Validate(command);

            Assert.False(result.IsValid);
            Assert.Contains(result.Errors, e => e.ErrorMessage.Contains("Purchase amount must be greater than zero") || e.PropertyName == "PurchaseAmount");
        }
    }
}
