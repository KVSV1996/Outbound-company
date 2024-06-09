using Outbound_company.Models;

namespace Outbound_company.Services.Interfaces
{
    public interface ICallsManagementService
    {
        void Start(OutboundCompany company, NumberPool numberPool, int maximumCountOfCalls);
        void Stop();
    }
}
