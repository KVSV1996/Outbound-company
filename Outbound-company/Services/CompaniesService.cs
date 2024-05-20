using Outbound_company.Models;
using Outbound_company.Repository;

namespace Outbound_company.Services
{
    public class CompaniesService : ICompaniesService
    {
        ICompaniesRepository companiesRepository;
        public CompaniesService(ICompaniesRepository companiesRepository)
        {
            this.companiesRepository = companiesRepository ?? throw new ArgumentNullException(nameof(companiesRepository));
        }
        public IEnumerable<OutboundCompany> GetAllCompanies() => companiesRepository.GetAllCompanies();
        public OutboundCompany GetCompanyById(int id) => companiesRepository.GetCompanyById(id);

        public void InsertCompany(OutboundCompany outboundCompany)
        {
            companiesRepository.InsertCompany(outboundCompany);
            companiesRepository.Save();
        }
        public void DeleteCompany(int id)
        {
            companiesRepository.DeleteCompany(id);
            companiesRepository.Save();
        }
        public void UpdateCompany(OutboundCompany outboundCompany)
        {
            companiesRepository.UpdateCompany(outboundCompany);
            companiesRepository.Save();
        }
    }
}
