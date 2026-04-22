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

using GLMS.Web.Models;

namespace GLMS.Web.Patterns.Strategy
{
    public class DraftContractStrategy : IServiceRequestValidationStrategy
    {
        public string AppliesToStatus => "Draft";

        public ValidationResult Validate(Contract contract, ServiceRequest request)
        {
            // Draft contracts can accept requests, but TechMove policy caps them
            // at $10k USD until the contract is activated.
            if (request.CostUSD > 10000m)
                return ValidationResult.Fail("Draft contracts cannot accept requests above $10,000. Activate the contract first.");

            return ValidationResult.Success();
        }
    }
}
