using Outbound_company.Models;
using System.Threading.Tasks;

namespace Outbound_company.Services.Interfaces
{
    public interface ICallStatisticsService
    {
         Task<IEnumerable<CallStatistics>> GetAllAsync();
         Task<CallStatistics> GetByIdAsync(int id);
        Task<IEnumerable<CallStatistics>> GetStatysticByCompanyIdAsync(int companyId);
        Task<CallStatistics> GetLatestByCompanyIdAsync(int companyId);
         Task AddAsync(CallStatistics callStatistics) ;
         Task UpdateAsync(CallStatistics callStatistics);
        Task DeleteStatysticByCompanyIdAsync(int companyId);
    }
}
