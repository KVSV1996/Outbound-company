using Outbound_company.Models;
using Outbound_company.Repository.Interface;
using Outbound_company.Services.Interfaces;

namespace Outbound_company.Services
{
    public class CompaniesService : ICompaniesService
    {
        private readonly ICompaniesRepository companiesRepository;

        public CompaniesService(ICompaniesRepository companiesRepository)
        {
            this.companiesRepository = companiesRepository ?? throw new ArgumentNullException(nameof(companiesRepository));
        }

        public async Task<IEnumerable<OutboundCompany>> GetAllCompaniesAsync()
        {
            return await companiesRepository.GetAllCompaniesAsync();
        }

        public async Task<OutboundCompany> GetCompanyByIdAsync(int id)
        {
            return await companiesRepository.GetCompanyByIdAsync(id);
        }

        public async Task InsertCompanyAsync(OutboundCompany outboundCompany)
        {
            await companiesRepository.InsertCompanyAsync(outboundCompany);
            companiesRepository.Save();
        }

        public async Task DeleteCompanyAsync(int id)
        {
            await companiesRepository.DeleteCompanyAsync(id);
            companiesRepository.Save();
        }

        public async Task UpdateCompanyAsync(OutboundCompany outboundCompany)
        {
            await companiesRepository.UpdateCompanyAsync(outboundCompany);
            companiesRepository.Save();
        }
    }
}
