/*
 * Pro C# 10 with .NET 6 Foundational Principles and Practices in Programming
 * Eleventh Edition
 * Andrew Troelsen, Philip Japikse
 * Apress, 2022
 */

using GLMS.Web.Models;

namespace GLMS.Web.Services
{
    //Validator to use with strategy pattern.
    public static class WorkflowValidator
    {
        private static readonly HashSet<string> BlockedStatuses =
            new(StringComparer.OrdinalIgnoreCase) { "Expired", "On Hold" };

        public static bool CanCreateServiceRequest(Contract contract)
        {
            if (contract == null) return false;
            return !BlockedStatuses.Contains(contract.Status);
        }

        public static string GetBlockedReason(Contract contract)
        {
            if (contract == null) return "Contract not found.";
            return $"Service requests cannot be raised on a contract with status '{contract.Status}'.";
        }
    }
}
