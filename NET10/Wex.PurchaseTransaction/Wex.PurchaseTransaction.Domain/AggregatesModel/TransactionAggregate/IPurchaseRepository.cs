namespace Wex.PurchaseTransaction.Domain.AggregatesModel.TransactionAggregate
{
    /// <summary>
    /// Purchase Repository Interface. Defines the contract for a repository that manages Purchase entities, 
    /// providing methods for adding new purchases and retrieving existing purchases with pagination, sorting, and filtering capabilities.
    /// </summary>
    public interface IPurchaseRepository
    {
        /// <summary>
        /// Asynchronously adds a new purchase to the repository.
        /// </summary>
        /// <param name="purchase">The purchase to add. This parameter must not be null and should contain valid purchase details.</param>
        /// <param name="cancellationToken">A cancellation token that can be used to cancel the operation if needed.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains the created purchase.</returns>
        Task<Purchase> AddAsync(Purchase purchase, CancellationToken cancellationToken);

        /// <summary>
        /// Asynchronously retrieves a purchase by its unique identifier.
        /// </summary>
        /// <param name="purchaseId">The unique identifier of the purchase to retrieve. Must be a positive value.</param>
        /// <param name="cancellationToken">A cancellation token that can be used to cancel the operation if needed.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains the purchase if found.</returns>
        Task<Purchase?> GetByIdAsync(long purchaseId, CancellationToken cancellationToken);

        /// <summary>
        /// Asynchronously retrieves a paginated collection of purchases that match the specified sorting and filtering
        /// criteria.
        /// </summary>
        /// <param name="page">The one-based index of the page to retrieve. Must be greater than or equal to 1.</param>
        /// <param name="pageSize">The number of purchases to include in each page. Must be greater than 0.</param>
        /// <param name="sortModel">A string specifying the sorting criteria to apply to the purchases, such as a field name or sort expression.</param>
        /// <param name="filterModel">A string specifying the filtering criteria to apply to the purchases, such as a filter expression or condition.</param>
        Task<IAsyncEnumerable<Purchase>> GetPurchasesAsync(int page, int pageSize, string sortModel, string filterModel);
    }
}
