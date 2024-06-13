using Microsoft.EntityFrameworkCore;
using Outbound_company.Models;

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
        public DbSet<BlackListNumber> BlackListNumbers { get; set; }
        public DbSet<User> Users { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<OutboundCompany>()
            .Property(o => o.Id)
            .ValueGeneratedOnAdd();

            modelBuilder.Entity<NumberPool>()
                .Property(np => np.Id)
                .ValueGeneratedOnAdd();

            modelBuilder.Entity<PhoneNumber>()
                .Property(pn => pn.Id)
                .ValueGeneratedOnAdd();

            modelBuilder.Entity<CallStatus>()
                .Property(cs => cs.Id)
                .ValueGeneratedOnAdd();

            modelBuilder.Entity<CallStatistics>()
                .Property(cs => cs.Id)
                .ValueGeneratedOnAdd();

            modelBuilder.Entity<BlackListNumber>()
                .Property(bln => bln.Id)
                .ValueGeneratedOnAdd();

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
