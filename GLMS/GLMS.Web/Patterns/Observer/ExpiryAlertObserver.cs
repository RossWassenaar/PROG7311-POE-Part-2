/*
 * Pro C# 10 with .NET 6 Foundational Principles and Practices in Programming
 * Eleventh Edition
 * Andrew Troelsen, Philip Japikse
 * Apress, 2022
 */
namespace GLMS.Web.Patterns.Observer
{
    public class ExpiryAlertObserver : IContractStatusObserver
    {
        private readonly ILogger<ExpiryAlertObserver> _logger;

        public ExpiryAlertObserver(ILogger<ExpiryAlertObserver> logger)
        {
            _logger = logger;
        }

        public Task OnStatusChangedAsync(ContractStatusChangedEvent evt)
        {
            if (string.Equals(evt.NewStatus, "Expired", StringComparison.OrdinalIgnoreCase))
            {
                _logger.LogWarning(
                    "[EXPIRY-ALERT] Contract {ContractId} for {Client} just flipped to Expired. Notify ops desk.",
                    evt.ContractId, evt.ClientName ?? "unknown");
            }
            return Task.CompletedTask;
        }
    }
}
