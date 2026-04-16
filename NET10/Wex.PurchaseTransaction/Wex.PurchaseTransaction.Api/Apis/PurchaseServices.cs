namespace Wex.PurchaseTransaction.Api.Apis
{
    using Cortex.Mediator;
    using Microsoft.Extensions.Logging;

    /// <summary>
    ///  Injects the necessary services for handling purchase-related operations, 
    ///  including the Mediator for sending commands and queries, and a Logger for logging activities within the purchase services. 
    ///  This class serves as a container for these dependencies, 
    ///  allowing them to be easily accessed and utilized in the API endpoints that manage purchases. 
    ///  By centralizing these services, it promotes cleaner code and better separation of concerns within the API layer.
    /// </summary>
    /// <param name="mediator">The mediator instance used for sending commands and queries.</param>
    /// <param name="logger">The logger instance used for logging activities within the purchase services.</param>
    public class PurchaseServices(IMediator mediator, ILogger<PurchaseServices> logger)
    {
        public IMediator Mediator { get; set; } = mediator;
        public ILogger<PurchaseServices> Logger { get; } = logger;
    }
}
