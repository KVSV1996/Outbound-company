using Outbound_company.Models;

namespace Outbound_company.Services.Interfaces
{
    public interface IBlackListNumberService
    {
        Task<IEnumerable<BlackListNumber>> GetAllAsync(int pageNumber, int pageSize);
        Task<BlackListNumber> GetByIdAsync(int id);
        Task AddAsync(BlackListNumber blackListNumber);
        Task UpdateAsync(BlackListNumber blackListNumber);
        Task DeleteAsync(int id);
        Task<int> GetCountAsync();
    }
}
