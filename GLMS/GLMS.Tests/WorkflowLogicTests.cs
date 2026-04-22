using GLMS.Web.Models;
using GLMS.Web.Services;
using Xunit;

namespace GLMS.Tests
{
    public class WorkflowLogicTests
    {
        [Fact]
        public void CanCreateServiceRequest_ActiveContract_ReturnsTrue()
        {
            var contract = new Contract { Status = "Active" };
            Assert.True(WorkflowValidator.CanCreateServiceRequest(contract));
        }

        [Fact]
        public void CanCreateServiceRequest_DraftContract_ReturnsTrue()
        {
            var contract = new Contract { Status = "Draft" };
            Assert.True(WorkflowValidator.CanCreateServiceRequest(contract));
        }

        [Fact]
        public void CanCreateServiceRequest_ExpiredContract_ReturnsFalse()
        {
            var contract = new Contract { Status = "Expired" };
            Assert.False(WorkflowValidator.CanCreateServiceRequest(contract));
        }

        [Fact]
        public void CanCreateServiceRequest_OnHoldContract_ReturnsFalse()
        {
            var contract = new Contract { Status = "On Hold" };
            Assert.False(WorkflowValidator.CanCreateServiceRequest(contract));
        }

        [Fact]
        public void CanCreateServiceRequest_NullContract_ReturnsFalse()
        {
            Assert.False(WorkflowValidator.CanCreateServiceRequest(null!));
        }

        [Fact]
        public void GetBlockedReason_ExpiredContract_ContainsExpired()
        {
            var contract = new Contract { Status = "Expired" };
            var reason = WorkflowValidator.GetBlockedReason(contract);
            Assert.Contains("Expired", reason);
        }

        [Fact]
        public void GetBlockedReason_NullContract_ReturnsMessage()
        {
            var reason = WorkflowValidator.GetBlockedReason(null!);
            Assert.False(string.IsNullOrEmpty(reason));
        }
    }
}
