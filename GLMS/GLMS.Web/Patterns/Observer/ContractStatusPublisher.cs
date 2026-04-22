/*
 * Pro C# 10 with .NET 6 Foundational Principles and Practices in Programming
 * Eleventh Edition
 * Andrew Troelsen, Philip Japikse
 * Apress, 2022
 */

namespace GLMS.Web.Patterns.Observer
{
    public interface IContractStatusPublisher
    {
        Task PublishAsync(ContractStatusChangedEvent evt);
    }

    public class ContractStatusPublisher : IContractStatusPublisher
    {
        private readonly IEnumerable<IContractStatusObserver> _observers;

        public ContractStatusPublisher(IEnumerable<IContractStatusObserver> observers)
        {
            _observers = observers;
        }

        public async Task PublishAsync(ContractStatusChangedEvent evt)
        {
            foreach (var observer in _observers)
            {

                try { await observer.OnStatusChangedAsync(evt); }
                catch { }
            }
        }
    }
}
