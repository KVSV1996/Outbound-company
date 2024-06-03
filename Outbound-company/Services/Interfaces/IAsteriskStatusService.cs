using Outbound_company.Models;

namespace Outbound_company.Services.Interfaces
{
    public interface IAsteriskStatusService
    {
        Task UpdateStatusAsync();
        AsteriskStatusModel GetStatus();
    }
}
