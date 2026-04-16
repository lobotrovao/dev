namespace Wex.PurchaseTransaction.Infrastructure.Databases
{
    using Microsoft.EntityFrameworkCore;
    using Wex.PurchaseTransaction.Domain.AggregatesModel.TransactionAggregate;
    using Wex.PurchaseTransaction.Infrastructure.EntityConfigurations;

    /// <summary>
    /// TransactionDbContext for EF Core. Represents the database context for the Transaction domain, providing access to the Transactions table and configuring 
    /// the entity mappings using the TransactionEntityTypeConfiguration class.
    /// </summary>
    public class PurchaseDbContext : DbContext
    {
        /// <summary>
        /// Configures the model by applying the entity type configurations for Transaction entities, 
        /// setting the default schema to "dbo", and ensuring that the database is created if it does not exist.
        /// </summary>
        /// <param name="modelBuilder"></param>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasDefaultSchema("dbo");
            modelBuilder.ApplyConfiguration(new PurchaseEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new ClientRequestEntityTypeConfiguration());
        }

        /// <summary>
        /// Gets or sets the DbSet of Transaction entities. 
        /// This property provides access to the Transactions table in the database, allowing for querying and saving instances of the Transaction entity.
        /// </summary>
        public DbSet<Purchase> Purchases { get; set; }

        /// <summary>
        /// Constructs a new instance of the TransactionDbContext class with the specified options.
        /// </summary>
        /// <param name="options"></param>
        public PurchaseDbContext(DbContextOptions<PurchaseDbContext> options) : base(options)
        {
            Database.EnsureCreated();
        }
    }
}
