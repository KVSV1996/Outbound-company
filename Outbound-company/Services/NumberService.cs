using Outbound_company.Models;
using Outbound_company.Repository;

namespace Outbound_company.Services
{
    public class NumberService : INumberService
    {
        private INumberRepository _numberRepository;
        public NumberService(INumberRepository numberRepository)
        {
            _numberRepository = numberRepository ?? throw new ArgumentNullException(nameof(numberRepository));
        }

        public IEnumerable<NumberPool> GetAllNumberPools() => _numberRepository.GetAllNumberPools();

        public NumberPool GetById(int id) => _numberRepository.GetById(id);

        public void InsertNumberPools(NumberPool numberPool)
        {
            _numberRepository.InsertNumberPools(numberPool);
            _numberRepository.Save();
        }

        public void DeleteNumberPools(int id)
        {
            _numberRepository.DeleteNumberPools(id);
            _numberRepository.Save();
        }

        public void UpdateNumberPools(NumberPool numberPool)
        {
            _numberRepository.UpdateNumberPools(numberPool);
            _numberRepository.Save();
        }
    }
}
