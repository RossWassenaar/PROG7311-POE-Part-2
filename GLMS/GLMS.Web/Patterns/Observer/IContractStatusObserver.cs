/*
 * Pro C# 10 with .NET 6 Foundational Principles and Practices in Programming
 * Eleventh Edition
 * Andrew Troelsen, Philip Japikse
 * Apress, 2022
 */
using GLMS.Web.Models;

namespace GLMS.Web.Patterns.Observer
{

    public record ContractStatusChangedEvent(
        int ContractId,
        string? ClientName,
        string OldStatus,
        string NewStatus,
        DateTime ChangedAtUtc);

    public interface IContractStatusObserver
    {
        Task OnStatusChangedAsync(ContractStatusChangedEvent evt);
    }
}
