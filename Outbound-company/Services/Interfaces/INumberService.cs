using Outbound_company.Models;

namespace Outbound_company.Services.Interfaces
{
    public interface INumberService
    {
        IEnumerable<NumberPool> GetAllNumberPools();
        NumberPool GetById(int id);
        Task<List<PhoneNumber>> GetPhoneNumbersStartingFromAsync(int numberPoolId, int startingPhoneId);
        void InsertNumberPools(NumberPool numberPool);
        void DeleteNumberPools(int id);
        void UpdateNumberPools(NumberPool numberPool);
    }
}
