/*
 * Pro C# 10 with .NET 6 Foundational Principles and Practices in Programming
 * Eleventh Edition
 * Andrew Troelsen, Philip Japikse
 * Apress, 2022
 */

namespace GLMS.Web.Patterns.Factory
{

    public interface IContractFactoryProvider
    {
        IContractFactory GetFactory(string serviceLevel);
    }

    public class ContractFactoryProvider : IContractFactoryProvider
    {
        private readonly Dictionary<string, IContractFactory> _factories;

        public ContractFactoryProvider(IEnumerable<IContractFactory> factories)
        {
            _factories = factories.ToDictionary(
                f => f.ServiceLevel,
                StringComparer.OrdinalIgnoreCase);
        }

        public IContractFactory GetFactory(string serviceLevel)
        {
            if (string.IsNullOrWhiteSpace(serviceLevel))
                throw new ArgumentException("Service level is required.", nameof(serviceLevel));

            if (!_factories.TryGetValue(serviceLevel, out var factory))
                throw new ArgumentException($"Unknown service level '{serviceLevel}'.", nameof(serviceLevel));

            return factory;
        }
    }
}
