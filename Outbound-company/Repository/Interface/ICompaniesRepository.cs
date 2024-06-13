using Outbound_company.Models;

namespace Outbound_company.Repository.Interface
{
    public interface ICompaniesRepository
    {
        Task<IEnumerable<OutboundCompany>> GetAllCompaniesAsync();
        Task<OutboundCompany> GetCompanyByIdAsync(int id);
        Task InsertCompanyAsync(OutboundCompany outboundCompany);
        Task DeleteCompanyAsync(int id);
        Task UpdateCompanyAsync(OutboundCompany outboundCompany);
        void Save();
    }
}
