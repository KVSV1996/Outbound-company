using Outbound_company.Models;

namespace Outbound_company.Repository
{
    public interface ICompaniesRepository
    {
        IEnumerable<OutboundCompany> GetAllCompanies();
        OutboundCompany GetCompanyById(int id);
        void InsertCompany(OutboundCompany outboundCompany);
        void DeleteCompany(int id);
        void UpdateCompany(OutboundCompany outboundCompany);
        void Save();
    }
}
