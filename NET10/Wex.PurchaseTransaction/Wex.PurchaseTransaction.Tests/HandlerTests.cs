namespace Wex.PurchaseTransaction.Tests
{
    using System.Reflection;
    using Wex.PurchaseTransaction.Domain.AggregatesModel.TransactionAggregate;
    using Wex.PurchaseTransaction.Application.Commands.CreatePurchase;
    using Wex.PurchaseTransaction.Application.Queries.Purchase;
    using Wex.PurchaseTransaction.Application.Services.Exchange;

    public class CreatePurchaseCommandHandlerTests
    {
        [Fact]
        public async Task Handle_ValidCommand_CreatesPurchaseAndReturnsId()
        {
            // Arrange
            var repo = new FakePurchaseRepository();
            var handler = new CreatePurchaseCommandHandler(repo);
            var command = new CreatePurchaseCommand("desc", DateTime.UtcNow.AddDays(-1), 15.5m);

            // Act
            var result = await handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.Equal(1, result);
        }
    }

    public class GetPurchaseByIdQueryHandlerTests
    {
        [Fact]
        public async Task Handle_PurchaseExistsAndExchangeAvailable_ReturnsConvertedDto()
        {
            // Arrange
            var purchase = new Purchase("desc", new DateTime(2023, 1, 1), 10m);
            // set id
            typeof(Purchase).GetProperty("Id", BindingFlags.Instance | BindingFlags.Public)!.SetValue(purchase, 5L);

            var repo = new FakePurchaseRepository(new[] { purchase });

            var data = new Data
            {
                RecordDate = new DateTime(2023, 01, 02),
                CountryCurrencyDesc = "USD",
                ExchangeRate = 2m
            };

            var exchange = new FakeExchangeService(new List<Data> { data });

            var handler = new GetPurchaseByIdQueryHandler(repo, exchange);
            var query = new GetPurchaseByIdQuery(5, "USD");

            // Act
            var result = await handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.Equal(5, result.Id);
            Assert.NotNull(result.ExchangeRate);
            Assert.Equal("USD", result.ExchangeRate!.Currency);
            Assert.Equal(2m, result.ExchangeRate.Rate);
            Assert.Equal(20.00m, result.ExchangeRate.ConvertedAmount);
        }

        [Fact]
        public async Task Handle_PurchaseExistsButNoExchange_ThrowsArgumentException()
        {
            // Arrange
            var purchase = new Purchase("desc", new DateTime(2023, 1, 1), 10m);
            typeof(Purchase).GetProperty("Id", BindingFlags.Instance | BindingFlags.Public)!.SetValue(purchase, 7L);
            var repo = new FakePurchaseRepository(new[] { purchase });
            var exchange = new FakeExchangeService(new List<Data>());

            var handler = new GetPurchaseByIdQueryHandler(repo, exchange);
            var query = new GetPurchaseByIdQuery(7, "EUR");

            // Act & Assert
            await Assert.ThrowsAsync<ArgumentException>(() => handler.Handle(query, CancellationToken.None));
        }
    }

    public class GetPurchasesQueryHandlerTests
    {
        [Fact]
        public async Task Handle_ReturnsPaginatedResponse()
        {
            // Arrange
            var purchases = Enumerable.Range(1, 5).Select(i =>
            {
                var p = new Purchase($"desc{i}", DateTime.UtcNow.AddDays(-i), 1m * i);
                typeof(Purchase).GetProperty("Id", BindingFlags.Instance | BindingFlags.Public)!.SetValue(p, (long)i);
                return p;
            }).ToList();

            var repo = new FakePurchaseRepository(purchases);
            var handler = new GetPurchaseQueryHandler(repo);

            var query = new GetPurchasesQuery(new Wex.PurchaseTransaction.Application.SeedWork.PaginationModel(0, 2), string.Empty, string.Empty);

            // Act
            var result = await handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.Equal(2, result.Data.Count);
            Assert.Equal(5, result.Total);
            Assert.Equal(0, result.Page);
            Assert.Equal(2, result.PageSize);
        }
    }

    // Simple in-memory fake implementations
    internal class FakePurchaseRepository : IPurchaseRepository
    {
        private readonly List<Purchase> _store;

        public FakePurchaseRepository(IEnumerable<Purchase>? seed = null)
        {
            _store = seed?.ToList() ?? new List<Purchase>();
        }

        public Task<Purchase> AddAsync(Purchase purchase, CancellationToken cancellationToken)
        {
            // assign id
            var prop = typeof(Purchase).GetProperty("Id", BindingFlags.Instance | BindingFlags.Public)!
                ;
            prop.SetValue(purchase, _store.Count + 1L);
            _store.Add(purchase);
            return Task.FromResult(purchase);
        }

        public Task<Purchase?> GetByIdAsync(long purchaseId, CancellationToken cancellationToken)
        {
            var found = _store.FirstOrDefault(x => x.Id == purchaseId);
            return Task.FromResult(found);
        }

        public Task<IAsyncEnumerable<Purchase>> GetPurchasesAsync(int page, int pageSize, string sortModel, string filterModel)
        {
            IAsyncEnumerable<Purchase> GetAsync()
            {
                return ToAsync(_store);
            }

            return Task.FromResult(GetAsync());
        }

        private static async IAsyncEnumerable<Purchase> ToAsync(IEnumerable<Purchase> items)
        {
            foreach (var item in items)
            {
                yield return item;
                await Task.Yield();
            }
        }
    }

    internal class FakeExchangeService : IExchangeService
    {
        private readonly List<Data> _data;
        public FakeExchangeService(List<Data> data) => _data = data;

        public Task<ExchangeDto?> GetExchangeRatesAsync(string date, string currency, string? relativeUrl = null, CancellationToken cancellationToken = default)
        {
            // not used in these tests
            return Task.FromResult<ExchangeDto?>(null);
        }

        public Task<List<Data>> GetAllExchangeRatesAsync(string date, string currency, CancellationToken cancellationToken = default)
        {
            return Task.FromResult(_data);
        }
    }
}
