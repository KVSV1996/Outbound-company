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

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<OutboundCompany>()
                .HasOne(c => c.NumberPool)
                .WithMany()
                .HasForeignKey(c => c.NumberPoolId);

            modelBuilder.Entity<NumberPool>()
                .HasMany(p => p.PhoneNumbers)
                .WithOne(n => n.NumberPool)
                .HasForeignKey(n => n.NumberPoolId);
        }
    }
}
