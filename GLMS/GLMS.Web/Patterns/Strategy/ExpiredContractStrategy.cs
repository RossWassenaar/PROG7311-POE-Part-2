/*
 * Pro C# 10 with .NET 6 Foundational Principles and Practices in Programming
 * Eleventh Edition
 * Andrew Troelsen, Philip Japikse
 * Apress, 2022
 */
using GLMS.Web.Models;

namespace GLMS.Web.Patterns.Strategy
{
    public class ExpiredContractStrategy : IServiceRequestValidationStrategy
    {
        public string AppliesToStatus => "Expired";

        public ValidationResult Validate(Contract contract, ServiceRequest request) =>
            ValidationResult.Fail("Service requests cannot be raised against an Expired contract.");
    }
}
