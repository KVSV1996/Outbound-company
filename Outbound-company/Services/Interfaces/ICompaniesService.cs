using Outbound_company.Models;
using Outbound_company.Repository;

namespace Outbound_company.Services.Interfaces
{
    public interface ICompaniesService
    {
        Task<IEnumerable<OutboundCompany>> GetAllCompaniesAsync();
        Task<OutboundCompany> GetCompanyByIdAsync(int id);
        Task InsertCompanyAsync(OutboundCompany outboundCompany);
        Task DeleteCompanyAsync(int id);
        Task UpdateCompanyAsync(OutboundCompany outboundCompany);

    }
}
