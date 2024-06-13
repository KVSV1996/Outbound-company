using Outbound_company.Models;
using Outbound_company.Repository.Interface;
using Outbound_company.Services.Interfaces;

namespace Outbound_company.Services
{
    public class NumberService : INumberService
    {
        private readonly INumberRepository _numberRepository;

        public NumberService(INumberRepository numberRepository)
        {
            _numberRepository = numberRepository ?? throw new ArgumentNullException(nameof(numberRepository));
        }

        public async Task<IEnumerable<NumberPool>> GetAllNumberPoolsAsync() => await _numberRepository.GetAllNumberPoolsAsync();

        public async Task<NumberPool> GetByIdAsync(int id) => await _numberRepository.GetByIdAsync(id);

        public async Task<List<PhoneNumber>> GetPhoneNumbersStartingFromAsync(int numberPoolId, int startingPhoneId) => await _numberRepository.GetPhoneNumbersStartingFromAsync(numberPoolId, startingPhoneId);

        public async Task InsertNumberPoolsAsync(NumberPool numberPool)
        {
            await _numberRepository.InsertNumberPoolsAsync(numberPool);
            _numberRepository.Save();
        }

        public async Task DeleteNumberPoolsAsync(int id)
        {
            await _numberRepository.DeleteNumberPoolsAsync(id);
            _numberRepository.Save();
        }

        public async Task UpdateNumberPoolsAsync(NumberPool numberPool)
        {
            await _numberRepository.UpdateNumberPoolsAsync(numberPool);
            _numberRepository.Save();
        }
    }
}
