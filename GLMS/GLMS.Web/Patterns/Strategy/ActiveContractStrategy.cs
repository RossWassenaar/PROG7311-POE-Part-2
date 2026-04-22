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
    public class ActiveContractStrategy : IServiceRequestValidationStrategy
    {
        public string AppliesToStatus => "Active";

        public ValidationResult Validate(Contract contract, ServiceRequest request)
        {
            if (DateTime.UtcNow.Date > contract.EndDate.Date)
                return ValidationResult.Fail("Contract end date has passed. Please renew before raising requests.");

            if (request.CostUSD < 0)
                return ValidationResult.Fail("Cost cannot be negative.");

            return ValidationResult.Success();
        }
    }
}
