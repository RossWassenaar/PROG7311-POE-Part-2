/*
 * Pro C# 10 with .NET 6 Foundational Principles and Practices in Programming
 * Eleventh Edition
 * Andrew Troelsen, Philip Japikse
 * Apress, 2022
 */

using GLMS.Web.Patterns.Observer;

namespace GLMS.Tests
{

    public class ObserverPatternTests
    {
        private class RecordingObserver : IContractStatusObserver
        {
            public List<ContractStatusChangedEvent> Received { get; } = new();
            public Task OnStatusChangedAsync(ContractStatusChangedEvent evt)
            {
                Received.Add(evt);
                return Task.CompletedTask;
            }
        }

        private class ThrowingObserver : IContractStatusObserver
        {
            public Task OnStatusChangedAsync(ContractStatusChangedEvent evt) =>
                throw new InvalidOperationException("simulated observer failure");
        }

        [Fact]
        public async Task Publisher_NotifiesAllObservers()
        {
            var a = new RecordingObserver();
            var b = new RecordingObserver();
            var publisher = new ContractStatusPublisher(new IContractStatusObserver[] { a, b });

            var evt = new ContractStatusChangedEvent(7, "Apex", "Draft", "Active", DateTime.UtcNow);
            await publisher.PublishAsync(evt);

            Assert.Single(a.Received);
            Assert.Single(b.Received);
            Assert.Equal(7, a.Received[0].ContractId);
            Assert.Equal("Active", b.Received[0].NewStatus);
        }

        [Fact]
        public async Task Publisher_OneObserverThrows_OthersStillReceiveEvent()
        {
            var good = new RecordingObserver();
            var publisher = new ContractStatusPublisher(new IContractStatusObserver[]
            {
                new ThrowingObserver(),
                good
            });

            var evt = new ContractStatusChangedEvent(1, "X", "Draft", "Active", DateTime.UtcNow);
            await publisher.PublishAsync(evt);

            Assert.Single(good.Received);
        }

        [Fact]
        public async Task Publisher_NoObservers_DoesNotThrow()
        {
            var publisher = new ContractStatusPublisher(Array.Empty<IContractStatusObserver>());
            var evt = new ContractStatusChangedEvent(1, null, "Draft", "Active", DateTime.UtcNow);

            await publisher.PublishAsync(evt);
        }
    }
}
