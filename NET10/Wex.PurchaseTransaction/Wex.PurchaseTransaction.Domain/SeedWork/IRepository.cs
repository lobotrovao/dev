namespace Wex.PurchaseTransaction.Domain.SeedWork
{
    /// <summary>
    /// Interface representing a generic repository for aggregate roots in the domain. 
    /// This interface defines the contract for repositories that manage aggregate root entities,
    /// </summary>
    /// <typeparam name="T">Entity Types.</typeparam>
    public interface IRepository<T> where T : IAggregateRoot
    {
    }
}
