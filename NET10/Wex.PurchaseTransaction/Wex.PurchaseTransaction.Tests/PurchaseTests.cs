namespace Wex.PurchaseTransaction.Tests
{
    using Wex.PurchaseTransaction.Domain.AggregatesModel.TransactionAggregate;

    public class PurchaseTests
    {
        [Fact]
        public void Constructor_ValidValues_SetsPropertiesAndRoundsAmount()
        {
            // Arrange
            var description = "Test purchase";
            var date = new DateTime(2023, 01, 01);
            decimal amount = 10.125m; // should round to 10.13

            // Act
            var purchase = new Purchase(description, date, amount);

            // Assert
            Assert.Equal(description, purchase.Description);
            Assert.Equal(date, purchase.TransactionDate);
            Assert.Equal(10.13m, purchase.PurchaseAmount);
        }

        [Fact]
        public void Constructor_EmptyDescription_ThrowsArgumentException()
        {
            // Arrange, Act & Assert
            Assert.Throws<ArgumentException>(() => new Purchase(string.Empty, DateTime.UtcNow, 1m));
        }

        [Fact]
        public void Constructor_TooLongDescription_ThrowsArgumentException()
        {
            // Arrange
            var longDescription = new string('a', 51);

            // Act & Assert
            Assert.Throws<ArgumentException>(() => new Purchase(longDescription, DateTime.UtcNow, 1m));
        }

        [Fact]
        public void Constructor_DefaultDate_ThrowsArgumentException()
        {
            // Arrange, Act & Assert
            Assert.Throws<ArgumentException>(() => new Purchase("desc", default, 1m));
        }

        [Fact]
        public void Constructor_NonPositiveAmount_ThrowsArgumentException()
        {
            // Arrange, Act & Assert
            Assert.Throws<ArgumentException>(() => new Purchase("desc", DateTime.UtcNow, 0m));
        }
    }
}
