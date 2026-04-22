/*
 * Pro C# 10 with .NET 6 Foundational Principles and Practices in Programming
 * Eleventh Edition
 * Andrew Troelsen, Philip Japikse
 * Apress, 2022
 */

//Title: Design Patterns in C Sharp (C#)
//Author: GeeksforGeeks
//Date: 20 April 2026
//Availability: https://www.geeksforgeeks.org/system-design/design-patterns-in-c-sharp/

namespace GLMS.Web.Patterns.Observer
{
    public class AuditLogObserver : IContractStatusObserver
    {
        private readonly ILogger<AuditLogObserver> _logger;

        public AuditLogObserver(ILogger<AuditLogObserver> logger)
        {
            _logger = logger;
        }

        public Task OnStatusChangedAsync(ContractStatusChangedEvent evt)
        {
            _logger.LogInformation(
                "[AUDIT] Contract {ContractId} ({Client}) status changed: {Old} -> {New} at {Timestamp:O}",
                evt.ContractId, evt.ClientName ?? "unknown", evt.OldStatus, evt.NewStatus, evt.ChangedAtUtc);
            return Task.CompletedTask;
        }
    }
}
