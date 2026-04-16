namespace Wex.PurchaseTransaction.Application.SeedWork
{
    using System.Text.Json.Serialization;

    /// <summary>
    /// Represents a request that specifies pagination, sorting, and filtering criteria for retrieving a subset of data.
    /// </summary>
    /// <remarks>Use this record to encapsulate all parameters required for paginated data retrieval, ensuring
    /// consistent handling of pagination, sorting, and filtering across queries.</remarks>
    /// <param name="PaginationModel">A string that defines the pagination settings, such as the page number and page size, to control which portion
    /// of the data set is returned.</param>
    /// <param name="SortModel">A string that specifies the sorting criteria, including the fields to sort by and the sort direction.</param>
    /// <param name="FilterModel">A string that contains the filtering criteria to apply to the data set, determining which records are included
    /// in the result.</param>
    public record PaginationRequest([property: JsonPropertyName("paginationModel")] string PaginationModel, 
        [property: JsonPropertyName("sortModel")] string SortModel, [property: JsonPropertyName("filterModel")] string FilterModel);
    public record PaginationModel([property: JsonPropertyName("page")] int Page, [property: JsonPropertyName("pageSize")]  int PageSize);
    public record SortItem([property: JsonPropertyName("field")] string Field, [property: JsonPropertyName("sort")]  string Sort);
    public record FilterModel([property: JsonPropertyName("items")] List<FilterItem> Items, [property: JsonPropertyName("logicOperator")] string LogicOperator,
        [property: JsonPropertyName("quickFilterValues")] List<string> QuickFilterValues, [property: JsonPropertyName("quickFilterLogicOperator")]  string QuickFilterLogicOperator);
    public record FilterItem([property: JsonPropertyName("field")] string Field, 
        [property: JsonPropertyName("operator")]  string Operator, [property: JsonPropertyName("value")] string Value);
}
