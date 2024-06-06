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

        public IEnumerable<OutboundCompany> GetAllCompanies()
        {

            if (context.OutboundCompanies == null)
            {
                throw new NullReferenceException();
            }

            return context.OutboundCompanies;
        }

        public OutboundCompany GetCompanyById(int id)
        {
            if (context.OutboundCompanies == null)
            {
                throw new NullReferenceException();
            }

            return context.OutboundCompanies.Find(id);
        }

        public void InsertCompany(OutboundCompany outboundCompany)
        {
            if (outboundCompany == null)
            {
                throw new NullReferenceException();
            }

            context.OutboundCompanies.Add(outboundCompany);
        }

        public void DeleteCompany(int id)
        {
            OutboundCompany outboundCompany = context.OutboundCompanies.Find(id);

            if (outboundCompany == null)
            {
                throw new NullReferenceException();
            }
            context.OutboundCompanies.Remove(outboundCompany);
        }

        public void UpdateCompany(OutboundCompany outboundCompany)
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
