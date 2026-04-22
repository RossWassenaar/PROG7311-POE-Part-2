/*
 * Pro C# 10 with .NET 6 Foundational Principles and Practices in Programming
 * Eleventh Edition
 * Andrew Troelsen, Philip Japikse
 * Apress, 2022
 */

using GLMS.Web.Models;
using GLMS.Web.Patterns.Strategy;

namespace GLMS.Tests
{

    public class ValidationStrategyTests
    {
        private static Contract MakeContract(string status, DateTime? end = null) => new()
        {
            Id = 1,
            Status = status,
            StartDate = new DateTime(2026, 1, 1),
            EndDate = end ?? new DateTime(2030, 1, 1),
            ServiceLevel = "Standard"
        };

        private static ServiceRequest MakeRequest(decimal costUsd = 500m) => new()
        {
            ContractId = 1,
            Description = "Test",
            CostUSD = costUsd,
            Status = "Pending"
        };

        [Fact]
        public void ActiveStrategy_ValidRequest_Passes()
        {
            var strategy = new ActiveContractStrategy();
            var result = strategy.Validate(MakeContract("Active"), MakeRequest());

            Assert.True(result.IsValid);
        }

        [Fact]
        public void ActiveStrategy_ContractAlreadyEnded_Fails()
        {
            var strategy = new ActiveContractStrategy();
            var result = strategy.Validate(
                MakeContract("Active", end: new DateTime(2020, 1, 1)),
                MakeRequest());

            Assert.False(result.IsValid);
        }

        [Fact]
        public void DraftStrategy_LargeCost_Fails()
        {
            var strategy = new DraftContractStrategy();
            var result = strategy.Validate(MakeContract("Draft"), MakeRequest(costUsd: 50_000m));

            Assert.False(result.IsValid);
        }

        [Fact]
        public void Selector_RoutesToExpiredStrategy_AndBlocksRequest()
        {
            var selector = new ValidationStrategySelector(new IServiceRequestValidationStrategy[]
            {
                new ActiveContractStrategy(),
                new DraftContractStrategy(),
                new ExpiredContractStrategy(),
                new OnHoldContractStrategy()
            });

            var result = selector.Validate(MakeContract("Expired"), MakeRequest());

            Assert.False(result.IsValid);
            Assert.Contains("Expired", result.ErrorMessage);
        }
    }
}
