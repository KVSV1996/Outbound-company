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
        public DbSet<NumberPool> NumberPools { get; set; }
        public DbSet <PhoneNumber> PhoneNumbers { get; set; }
        public DbSet<CallStatus> CallStatuses { get; set; }
        public DbSet<CallStatistics> CallStatistics { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<NumberPool>()
                .HasMany(p => p.PhoneNumbers)
                .WithOne(n => n.NumberPool)
                .HasForeignKey(n => n.NumberPoolId);

            modelBuilder.Entity<CallStatistics>()
                .HasOne(cs => cs.Company)
                .WithMany()
                .HasForeignKey(cs => cs.CompanyId);

            modelBuilder.Entity<CallStatistics>()
                .HasOne(cs => cs.PhoneNumber)
                .WithMany()
                .HasForeignKey(cs => cs.PhoneNumberId);

            modelBuilder.Entity<CallStatistics>()
                .HasOne(cs => cs.CallStatus)
                .WithMany()
                .HasForeignKey(cs => cs.CallStatusId);
        }
    }
}
