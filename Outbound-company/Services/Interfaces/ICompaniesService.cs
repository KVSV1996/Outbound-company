using Outbound_company.Models;
using Outbound_company.Repository;

namespace Outbound_company.Services.Interfaces
{
    public interface ICompaniesService
    {
        IEnumerable<OutboundCompany> GetAllCompanies();
        OutboundCompany GetCompanyById(int id);
        void InsertCompany(OutboundCompany outboundCompany);
        void DeleteCompany(int id);
        void UpdateCompany(OutboundCompany outboundCompany);

    }
}
