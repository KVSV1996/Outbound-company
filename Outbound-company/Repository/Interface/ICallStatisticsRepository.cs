using Outbound_company.Models;

namespace Outbound_company.Repository.Interface
{
    public interface ICallStatisticsRepository
    {
        Task<IEnumerable<CallStatistics>> GetAllAsync();
        Task<CallStatistics> GetByIdAsync(int id);
        Task<IEnumerable<CallStatistics>> GetStatysticByCompanyIdAsync(int companyId);
        Task<CallStatistics> GetLatestByCompanyIdAsync(int companyId);
        Task AddAsync(CallStatistics callStatistics);
        Task UpdateAsync(CallStatistics callStatistics);
        Task DeleteStatysticByCompanyIdAsync(int companyId);
    }
}
