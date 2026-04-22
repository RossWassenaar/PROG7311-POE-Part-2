/*
 * Pro C# 10 with .NET 6 Foundational Principles and Practices in Programming
 * Eleventh Edition
 * Andrew Troelsen, Philip Japikse
 * Apress, 2022
 */

using GLMS.Web.Models;

namespace GLMS.Web.Patterns.Factory
{
    public class StandardContractFactory : IContractFactory
    {
        public string ServiceLevel => "Standard";

        public Contract Create(int clientId, DateTime startDate, DateTime endDate)
        {
            if (endDate <= startDate)
                throw new ArgumentException("End date must be after start date.");

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
