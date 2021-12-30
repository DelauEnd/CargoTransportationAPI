using Entities.Configuration;
using Entities.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Entities
{
    public class RepositoryContext : IdentityDbContext<User>
    {
        public DbSet<Cargo> Cargoes { get; set; }
        public DbSet<CargoCategory> Categories { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<Route> Routes { get; set; }
        public DbSet<Transport> Transports { get; set; }
        public DbSet<Customer> Customers { get; set; }

        public RepositoryContext(DbContextOptions options)
            : base(options)
        {
            Database.EnsureCreated();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            ApplyConfigurations(modelBuilder);

            AddProcedures();
        }

        private void AddProcedures()
        {
            TryToExecQuery(StoredProcedures.AssignCargoToRoute);
        }

        private void TryToExecQuery(string query)
        {
            try
            {
                this.Database.ExecuteSqlRaw(query);
            }
            catch { }
        }

        private void ApplyConfigurations(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new RoleConfiguration());
            modelBuilder.ApplyConfiguration(new CustomerConfiguration());
            modelBuilder.ApplyConfiguration(new TransportConfiguration());
            modelBuilder.ApplyConfiguration(new CargoCategoryConfiguration());
            modelBuilder.ApplyConfiguration(new OrderConfiguration());
            modelBuilder.ApplyConfiguration(new CargoConfiguration());
            modelBuilder.ApplyConfiguration(new RouteConfiguration());
        }
    }
}
