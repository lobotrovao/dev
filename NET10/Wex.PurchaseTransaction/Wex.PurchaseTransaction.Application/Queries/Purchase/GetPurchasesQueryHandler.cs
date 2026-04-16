namespace Wex.PurchaseTransaction.Application.Queries.Purchase
{
    using Cortex.Mediator.Queries;
    using Wex.PurchaseTransaction.Application.SeedWork;
    using Wex.PurchaseTransaction.Domain.AggregatesModel.TransactionAggregate;

    public class GetPurchaseQueryHandler(IPurchaseRepository purchaseRepository) : IQueryHandler<GetPurchasesQuery, PaginationResponse<PurchaseDto>>
    {
        public async Task<PaginationResponse<PurchaseDto>> Handle(GetPurchasesQuery query, CancellationToken cancellationToken)
        {
            var page = query.PaginationModel.Page;
            var pageSize = query.PaginationModel.PageSize;
            var result = await purchaseRepository.GetPurchasesAsync(page,pageSize, query.SortModel, query.FilterModel);

            var purchases = await result.Select(x =>  new PurchaseDto(x.Id, x.Description, x.TransactionDate, x.PurchaseAmount)).ToListAsync();

            if (page < 0) page = 0;
            if (pageSize <= 0) pageSize = 10;
            int startIndex = page * pageSize;
            var pages = purchases.Skip(startIndex).Take(pageSize).ToList();
            
            return new PaginationResponse<PurchaseDto>(pages, purchases.Count, page, pageSize);
        }
    }
}
