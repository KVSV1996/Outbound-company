using Microsoft.EntityFrameworkCore;
using Outbound_company.Context;
using Outbound_company.Models;
using Outbound_company.Repository.Interface;

namespace Outbound_company.Repository
{
    public class CompaniesRepository : ICompaniesRepository
    {
        private IApplicationDbContext context;

        public CompaniesRepository(IApplicationDbContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            this.context = context;
        }

        public async Task<IEnumerable<OutboundCompany>> GetAllCompaniesAsync()
        {
            if (context.OutboundCompanies == null)
            {
                throw new NullReferenceException();
            }

            return await context.OutboundCompanies.ToListAsync();
        }

        public async Task<OutboundCompany> GetCompanyByIdAsync(int id)
        {
            if (context.OutboundCompanies == null)
            {
                throw new NullReferenceException();
            }

            return await context.OutboundCompanies.FindAsync(id);
        }

        public async Task InsertCompanyAsync(OutboundCompany outboundCompany)
        {
            if (outboundCompany == null)
            {
                throw new NullReferenceException();
            }

            await context.OutboundCompanies.AddAsync(outboundCompany);
        }

        public async Task DeleteCompanyAsync(int id)
        {
            OutboundCompany outboundCompany = await context.OutboundCompanies.FindAsync(id);

            if (outboundCompany == null)
            {
                throw new NullReferenceException();
            }
            context.OutboundCompanies.Remove(outboundCompany);
        }

        public async Task UpdateCompanyAsync(OutboundCompany outboundCompany)
        {
            if (outboundCompany == null)
            {
                throw new NullReferenceException();
            }
            context.OutboundCompanies.Update(outboundCompany);
        }

        public void Save()
        {
            context.SaveChanges();
        }
    }
}
