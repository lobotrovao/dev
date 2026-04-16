namespace Wex.PurchaseTransaction.Infrastructure.Idempotency
{
    using Wex.PurchaseTransaction.Domain.Idempotency;
    using Wex.PurchaseTransaction.Infrastructure.Databases;

    public class RequestManager : IRequestManager
    {
        private readonly PurchaseDbContext _context;

        public RequestManager(PurchaseDbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task<bool> ExistAsync(Guid id)
        {
            var request = await _context.
                FindAsync<ClientRequest>(id);

            return request != null;
        }

        public async Task CreateRequestForCommandAsync<T>(Guid id)
        {
            var exists = await ExistAsync(id);

            var request = exists ?
                throw new ArgumentException($"Request with {id} already exists") :
                new ClientRequest()
                {
                    Id = id,
                    Name = typeof(T).Name,
                    Time = DateTime.UtcNow
                };

            _context.Add(request);

            await _context.SaveChangesAsync();
        }
    }
}