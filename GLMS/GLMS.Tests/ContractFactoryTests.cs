/*
 * Pro C# 10 with .NET 6 Foundational Principles and Practices in Programming
 * Eleventh Edition
 * Andrew Troelsen, Philip Japikse
 * Apress, 2022
 */

using GLMS.Web.Patterns.Factory;

namespace GLMS.Tests
{

    public class ContractFactoryTests
    {

        [Fact]
        public void StandardFactory_CreatesDraftContractWithCorrectServiceLevel()
        {
            var factory = new StandardContractFactory();

            var contract = factory.Create(
                clientId: 1,
                startDate: new DateTime(2026, 1, 1),
                endDate: new DateTime(2026, 6, 1));

            Assert.Equal("Standard", contract.ServiceLevel);
            Assert.Equal("Draft", contract.Status);
            Assert.Equal(1, contract.ClientId);
        }


        [Fact]
        public void PremiumFactory_TermShorterThan90Days_Throws()
        {
            var factory = new PremiumContractFactory();

            Assert.Throws<ArgumentException>(() => factory.Create(
                clientId: 1,
                startDate: new DateTime(2026, 1, 1),
                endDate: new DateTime(2026, 2, 1)));
        }


        [Fact]
        public void Provider_ResolvesByServiceLevel()
        {
            var provider = new ContractFactoryProvider(new IContractFactory[]
            {
                new StandardContractFactory(),
                new PremiumContractFactory(),
                new EnterpriseContractFactory()
            });

            Assert.IsType<StandardContractFactory>(provider.GetFactory("Standard"));
            Assert.IsType<PremiumContractFactory>(provider.GetFactory("Premium"));
            Assert.IsType<EnterpriseContractFactory>(provider.GetFactory("Enterprise"));
        }
    }
}
