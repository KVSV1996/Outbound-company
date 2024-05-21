using Outbound_company.Models;

namespace Outbound_company.Repository
{
    public interface INumberRepository
    {
        IEnumerable<NumberPool> GetAllNumberPools();
        NumberPool GetById(int id);
        void InsertNumberPools(NumberPool numberPool);
        void DeleteNumberPools(int id);
        void UpdateNumberPools(NumberPool numberPool);
        void Save();
    }
}
