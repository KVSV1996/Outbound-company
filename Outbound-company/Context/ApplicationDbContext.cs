using Microsoft.EntityFrameworkCore;
using Outbound_company.Models;
using System.Reflection.Emit;

namespace Outbound_company.Context
{
    public class ApplicationDbContext : DbContext, IApplicationDbContext
    {
        public ApplicationDbContext(DbContextOptions< ApplicationDbContext> options):base(options)
        {
            //Database.EnsureDeleted();
            //Database.EnsureCreated();
        }

        public DbSet<OutboundCompany> OutboundCompanies { get; set; }
        public DbSet <PhoneNumber> PhoneNumbers { get; set; }

        //protected override void OnModelCreating(ModelBuilder modelBuilder)
        //{
        //    base.OnModelCreating(modelBuilder);

        //    modelBuilder.Entity<OutboundCompany>()
        //        .HasMany(c => c.PhoneNumbers)
        //        .WithOne(p => p.OutboundCompany)
        //        .HasForeignKey(p => p.OutboundCompanyId);
        //}
    }
}
