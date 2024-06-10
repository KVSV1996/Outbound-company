using Outbound_company.Models;

namespace Outbound_company.Services.Interfaces
{
    public interface ICallsManagementService
    {
        void Start(OutboundCompany company, NumberPool numberPool, IEnumerable<BlackListNumber> blackListNumber, int maximumCountOfCalls);
        void Stop();
    }
}
