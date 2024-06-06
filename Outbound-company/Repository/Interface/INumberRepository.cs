using Outbound_company.Models;

namespace Outbound_company.Repository.Interface
{
    public interface INumberRepository
    {
        IEnumerable<NumberPool> GetAllNumberPools();
        NumberPool GetById(int id);
        Task<List<PhoneNumber>> GetPhoneNumbersStartingFromAsync(int numberPoolId, int startingPhoneId);
        void InsertNumberPools(NumberPool numberPool);
        void DeleteNumberPools(int id);
        void UpdateNumberPools(NumberPool numberPool);
        void Save();
    }
}
