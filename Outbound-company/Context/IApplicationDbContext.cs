using Microsoft.EntityFrameworkCore;
using Outbound_company.Models;

namespace Outbound_company.Context
{
    public interface IApplicationDbContext
    {
        DbSet<OutboundCompany> OutboundCompanies { get; set; }
        DbSet<NumberPool> NumberPools { get; set; }
        DbSet<PhoneNumber> PhoneNumbers { get; set; }
        int SaveChanges();
    }
}
