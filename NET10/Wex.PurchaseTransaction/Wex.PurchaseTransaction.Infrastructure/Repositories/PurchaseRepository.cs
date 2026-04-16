namespace Wex.PurchaseTransaction.Infrastructure.Repositories
{
    using Microsoft.EntityFrameworkCore;
    using Wex.PurchaseTransaction.Domain.AggregatesModel.TransactionAggregate;
    using Wex.PurchaseTransaction.Infrastructure.Databases;
    using Wex.PurchaseTransaction.Infrastructure.Extensions;

    /// <inheritdoc/>
    public class PurchaseRepository(PurchaseDbContext context) : IPurchaseRepository
    {
        /// <inheritdoc/>
        public async Task<Purchase> AddAsync(Purchase purchase, CancellationToken cancellationToken)
        {
            context.Purchases.Add(purchase);
            await context.SaveChangesAsync(cancellationToken);
            return purchase;
        }

        /// <inheritdoc/>
        public Task<Purchase?> GetByIdAsync(long purchaseId, CancellationToken cancellationToken)
        {
            var result = context.Purchases.AsNoTracking().FirstOrDefaultAsync(p => p.Id == purchaseId, cancellationToken);
            return result;
        }

        /// <inheritdoc/>
        public async Task<IAsyncEnumerable<Purchase>> GetPurchasesAsync(int page, int pageSize, string sortModel, string filterModel)
        {
            var query = context.Purchases.AsQueryable();
            query = query.ApplyFilters(filterModel);
            query = query.ApplySorting(sortModel);

            var result = query.AsNoTracking().ToAsyncEnumerable();
            return result;
        }
    }
}
