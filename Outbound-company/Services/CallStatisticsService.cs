using Outbound_company.Models;
using Outbound_company.Repository.Interface;
using Outbound_company.Services.Interfaces;

namespace Outbound_company.Services
{
    public class CallStatisticsService : ICallStatisticsService
    {
        private readonly ICallStatisticsRepository _repository;

        public CallStatisticsService(ICallStatisticsRepository repository)
        {
            _repository = repository;
        }

        public async Task<IEnumerable<CallStatistics>> GetAllAsync() => await _repository.GetAllAsync();
        public async Task<CallStatistics> GetByIdAsync(int id) => await _repository.GetByIdAsync(id);
        public async Task<IEnumerable<CallStatistics>> GetStatysticByCompanyIdAsync(int companyId) => await _repository.GetStatysticByCompanyIdAsync(companyId);
        public async Task<CallStatistics> GetLatestByCompanyIdAsync(int companyId) => await _repository.GetLatestByCompanyIdAsync(companyId);
        public async Task AddAsync(CallStatistics callStatistics) => await _repository.AddAsync(callStatistics);
        public async Task UpdateAsync(CallStatistics callStatistics) => await _repository.UpdateAsync(callStatistics);
        public async Task DeleteStatysticByCompanyIdAsync(int companyId) => await _repository.DeleteStatysticByCompanyIdAsync(companyId);


    }
}
