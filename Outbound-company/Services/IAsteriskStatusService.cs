using Outbound_company.Models;

namespace Outbound_company.Services
{
    public interface IAsteriskStatusService
    {
        Task UpdateStatusAsync();
        AsteriskStatusModel GetStatus();
    }
}
