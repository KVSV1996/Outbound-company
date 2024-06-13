using Microsoft.EntityFrameworkCore;
using Outbound_company.Models;

namespace Outbound_company.Context
{
    public interface IApplicationDbContext
    {
        DbSet<OutboundCompany> OutboundCompanies { get; set; }
        DbSet<NumberPool> NumberPools { get; set; }
        DbSet<PhoneNumber> PhoneNumbers { get; set; }
        DbSet<CallStatus> CallStatuses { get; set; }
        DbSet<CallStatistics> CallStatistics { get; set; }
        DbSet<BlackListNumber> BlackListNumbers { get; set; }
        DbSet<User> Users { get; set; }
        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
        int SaveChanges();
    }
}
