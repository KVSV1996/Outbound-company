using Outbound_company.Models;
using Outbound_company.Repository.Interface;
using Outbound_company.Services.Interfaces;

namespace Outbound_company.Services
{
    public class BlackListNumberService : IBlackListNumberService
    {
        private readonly IBlackListNumberRepository _repository;

        public BlackListNumberService(IBlackListNumberRepository repository)
        {
            _repository = repository;
        }

        public async Task<IEnumerable<BlackListNumber>> GetAllByPagAsync(int pageNumber, int pageSize) => await _repository.GetAllByPagAsync(pageNumber, pageSize);
        public async Task<IEnumerable<BlackListNumber>> GetAllAsync() => await _repository.GetAllAsync();
        public async Task<BlackListNumber> GetByIdAsync(int id) => await _repository.GetByIdAsync(id);
        public async Task AddAsync(BlackListNumber blackListNumber) => await _repository.AddAsync(blackListNumber);
        public async Task UpdateAsync(BlackListNumber blackListNumber) => await _repository.UpdateAsync(blackListNumber);
        public async Task DeleteAsync(int id) => await _repository.DeleteAsync(id);
        public async Task<int> GetCountAsync() => await _repository.GetCountAsync();
    }
}
