/*
 * Pro C# 10 with .NET 6 Foundational Principles and Practices in Programming
 * Eleventh Edition
 * Andrew Troelsen, Philip Japikse
 * Apress, 2022
 */

using GLMS.Web.Models;

namespace GLMS.Web.Patterns.Strategy
{
    //Strategy Pattern GoF — Behavioral.
    public interface IServiceRequestValidationStrategy
    {
        string AppliesToStatus { get; }
        ValidationResult Validate(Contract contract, ServiceRequest request);
    }

    public record ValidationResult(bool IsValid, string? ErrorMessage)
    {
        public static ValidationResult Success() => new(true, null);
        public static ValidationResult Fail(string message) => new(false, message);
    }
}
