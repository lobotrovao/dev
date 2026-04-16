namespace Wex.PurchaseTransaction.Application.SeedWork
{
    /// <summary>
    /// Represents a paginated response that contains a collection of data items and associated pagination metadata.
    /// </summary>
    /// <remarks>Use this record to return paginated results from APIs or queries, enabling clients to
    /// efficiently navigate large datasets by requesting specific pages. The total count may be null if the data source
    /// does not support counting the total number of items.</remarks>
    /// <typeparam name="T">The type of the data items included in the paginated response.</typeparam>
    /// <param name="Data">The collection of data items returned for the current page.</param>
    /// <param name="Total">The total number of items available across all pages, or null if the total is not known.</param>
    /// <param name="Page">The current page number, starting from 1.</param>
    /// <param name="PageSize">The number of items included per page.</param>
    public record PaginationResponse<T>(IReadOnlyList<T> Data, long? Total, long Page, int PageSize);
}