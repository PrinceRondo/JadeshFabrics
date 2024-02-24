using JadeshGeneral.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JadeshGeneral.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
             : base(options)
        {
        }
        //public DbSet<UserProfile> UserProfiles { get; set; }
        //public DbSet<AuditLog> AuditLogs { get; set; }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            foreach (var entityType in builder.Model.GetEntityTypes())
            {
                var properties = entityType.ClrType.GetProperties().Where(p => p.PropertyType == typeof(decimal));


                foreach (var property in properties)
                {
                    builder.Entity(entityType.Name).Property(property.Name).HasColumnType("decimal(18,2)");
                }
                base.OnModelCreating(builder);

            }
        }
        public override int SaveChanges()
        {
            PerformEntityAudit();
            return base.SaveChanges();
        }

        //public override Task<int> SaveChangesAsync()
        //{
        //    PerformEntityAudit();
        //    return base.SaveChangesAsync();
        //}

        private void PerformEntityAudit()
        {
            foreach (var entry in ChangeTracker.Entries<BaseEntity>())
            {
                switch (entry.State)
                {
                    case EntityState.Added:
                        var currentDateTime = DateTime.Now;
                        entry.Entity.CreatedDate = currentDateTime;
                        entry.Entity.ModifiedDate = currentDateTime;
                        entry.Entity.IsDeleted = false;
                        break;

                    case EntityState.Modified:
                        entry.Entity.ModifiedDate = DateTime.Now;
                        break;

                    case EntityState.Deleted:
                        entry.State = EntityState.Modified;

                        entry.Entity.ModifiedDate = DateTime.Now;
                        entry.Entity.IsDeleted = true;
                        break;
                }
            }
        }
    }
}
