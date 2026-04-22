using GLMS.Web.Models;
using Microsoft.EntityFrameworkCore;

namespace GLMS.Web.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Client> Clients { get; set; }
        public DbSet<Contract> Contracts { get; set; }
        public DbSet<ServiceRequest> ServiceRequests { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Client>().HasData(
                new Client { Id = 1, Name = "Apex Freight Corp", ContactDetails = "apex@freight.com | +27 11 234 5678", Region = "Gauteng" },
                new Client { Id = 2, Name = "Blue Ocean Shipping", ContactDetails = "info@blueocean.co.za | +27 21 987 6543", Region = "Western Cape" },
                new Client { Id = 3, Name = "Savanna Logistics", ContactDetails = "ops@savanna.co.za | +27 31 456 7890", Region = "KwaZulu-Natal" }
            );

            modelBuilder.Entity<Contract>().HasData(
                new Contract
                {
                    Id = 1, ClientId = 1,
                    StartDate = new DateTime(2024, 1, 1), EndDate = new DateTime(2025, 12, 31),
                    Status = "Active", ServiceLevel = "Premium"
                },
                new Contract
                {
                    Id = 2, ClientId = 2,
                    StartDate = new DateTime(2023, 6, 1), EndDate = new DateTime(2024, 5, 31),
                    Status = "Expired", ServiceLevel = "Standard"
                },
                new Contract
                {
                    Id = 3, ClientId = 3,
                    StartDate = new DateTime(2025, 3, 1), EndDate = new DateTime(2026, 3, 1),
                    Status = "Active", ServiceLevel = "Enterprise"
                }
            );
        }
    }
}
