/*
 * Pro C# 10 with .NET 6 Foundational Principles and Practices in Programming
 * Eleventh Edition
 * Andrew Troelsen, Philip Japikse
 * Apress, 2022
 */

using GLMS.Web.Models;

namespace GLMS.Web.Patterns.Factory
{

    public interface IContractFactory
    {
        string ServiceLevel { get; }
        Contract Create(int clientId, DateTime startDate, DateTime endDate);
    }
}
