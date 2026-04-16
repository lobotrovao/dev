namespace Wex.PurchaseTransaction.Domain.Idempotency
{
    using System.ComponentModel.DataAnnotations;

    public class ClientRequest
    {
        public Guid Id { get; set; }
        [Required]
        public string Name { get; set; } = string.Empty;
        public DateTime Time { get; set; }
    }

}