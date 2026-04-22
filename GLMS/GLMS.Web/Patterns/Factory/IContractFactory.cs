/*
 * Pro C# 10 with .NET 6 Foundational Principles and Practices in Programming
 * Eleventh Edition
 * Andrew Troelsen, Philip Japikse
 * Apress, 2022
 */

//Title: Factory Design Pattern in C#
//Author: Dot Net Tutorials
//Date: 20 April
//Availability: https://dotnettutorials.net/lesson/factory-design-pattern-csharp/


using GLMS.Web.Models;

namespace GLMS.Web.Patterns.Factory
{

    public interface IContractFactory
    {
        string ServiceLevel { get; }
        Contract Create(int clientId, DateTime startDate, DateTime endDate);
    }
}
