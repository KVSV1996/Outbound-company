using Outbound_company.Models;

namespace Outbound_company.Repository.Interface
{
    public interface IBlackListNumberRepository
    {
        Task<IEnumerable<BlackListNumber>> GetAllByPagAsync(int pageNumber, int pageSize);
        Task<IEnumerable<BlackListNumber>> GetAllAsync();
        Task<BlackListNumber> GetByIdAsync(int id);
        Task AddAsync(BlackListNumber blackListNumber);
        Task UpdateAsync(BlackListNumber blackListNumber);
        Task DeleteAsync(int id);
        Task<int> GetCountAsync();
    }
}
