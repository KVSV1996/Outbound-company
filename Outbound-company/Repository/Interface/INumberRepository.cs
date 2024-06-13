using Outbound_company.Models;

namespace Outbound_company.Repository.Interface
{
    public interface INumberRepository
    {
        Task<IEnumerable<NumberPool>> GetAllNumberPoolsAsync();
        Task<NumberPool> GetByIdAsync(int id);
        Task<List<PhoneNumber>> GetPhoneNumbersStartingFromAsync(int numberPoolId, int startingPhoneId);
        Task InsertNumberPoolsAsync(NumberPool numberPool);
        Task DeleteNumberPoolsAsync(int id);
        Task UpdateNumberPoolsAsync(NumberPool numberPool);
        void Save();
    }
}
