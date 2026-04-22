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
    public interface IValidationStrategySelector
    {
        ValidationResult Validate(Contract contract, ServiceRequest request);
    }

    public class ValidationStrategySelector : IValidationStrategySelector
    {
        private readonly Dictionary<string, IServiceRequestValidationStrategy> _strategies;

        public ValidationStrategySelector(IEnumerable<IServiceRequestValidationStrategy> strategies)
        {
            _strategies = strategies.ToDictionary(
                s => s.AppliesToStatus,
                StringComparer.OrdinalIgnoreCase);
        }

        public ValidationResult Validate(Contract contract, ServiceRequest request)
        {
            if (contract == null)
                return ValidationResult.Fail("Contract not found.");

            if (!_strategies.TryGetValue(contract.Status, out var strategy))
                return ValidationResult.Fail($"No validation strategy registered for status '{contract.Status}'.");

            return strategy.Validate(contract, request);
        }
    }
}
