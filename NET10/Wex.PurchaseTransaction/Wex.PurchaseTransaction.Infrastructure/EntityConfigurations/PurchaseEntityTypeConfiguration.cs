namespace Wex.PurchaseTransaction.Infrastructure.EntityConfigurations
{
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;
    using Wex.PurchaseTransaction.Domain.AggregatesModel.TransactionAggregate;

    /// <summary>
    /// Transaction Entity Type Configuration for EF Core. Configures the Transaction entity, its properties and relationships to the database schema.
    /// </summary>
    public class PurchaseEntityTypeConfiguration : IEntityTypeConfiguration<Purchase>
    {
        /// <summary>
        /// Configures the Transaction entity for EF Core. Maps the Transaction properties to database columns, sets up primary key and relationships.
        /// </summary>
        /// <param name="builder">EntityTypeBuilder.</param>
        public void Configure(EntityTypeBuilder<Purchase> builder)
        {
            builder.ToTable("Transactions");
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id);
            builder.Property(x => x.Description).IsRequired();
            builder.Property(x => x.TransactionDate).IsRequired();
            builder.Property(x => x.PurchaseAmount).IsRequired();
        }
    }
}
