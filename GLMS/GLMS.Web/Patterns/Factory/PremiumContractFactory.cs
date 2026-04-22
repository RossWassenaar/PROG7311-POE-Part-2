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
    public class PremiumContractFactory : IContractFactory
    {
        public string ServiceLevel => "Premium";

        public Contract Create(int clientId, DateTime startDate, DateTime endDate)
        {
            if (endDate <= startDate)
                throw new ArgumentException("End date must be after start date.");

            if ((endDate - startDate).TotalDays < 90)
                throw new ArgumentException("Premium contracts require a minimum 90-day term.");

            return new Contract
            {
                ClientId = clientId,
                StartDate = startDate,
                EndDate = endDate,
                ServiceLevel = ServiceLevel,
                Status = "Draft"
            };
        }
    }
}
