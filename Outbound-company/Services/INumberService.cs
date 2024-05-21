using Outbound_company.Models;

namespace Outbound_company.Services
{
    public interface INumberService
    {
        IEnumerable<NumberPool> GetAllNumberPools();
        NumberPool GetById(int id);
        void InsertNumberPools(NumberPool numberPool);
        void DeleteNumberPools(int id);
        void UpdateNumberPools(NumberPool numberPool);
    }
}
