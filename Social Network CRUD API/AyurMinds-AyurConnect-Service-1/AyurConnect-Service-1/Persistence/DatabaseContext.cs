using Microsoft.EntityFrameworkCore;
using AyurConnect_Service_1.Models;

namespace AyurConnect_Service_1.Persistence
{
    public class DatabaseContext : DbContext
    {
        private readonly IConfiguration _configuration;

        public DbSet<Content> Contents { get; set; } = null!;
        public DbSet<Response> Responses { get; set; } = null!;

        public DatabaseContext(IConfiguration configuration) => _configuration = configuration;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            string databaseConnectionString = _configuration.GetConnectionString("MySQLDatabaseConnection")!;
            optionsBuilder.UseMySQL(databaseConnectionString);
        }

        protected override void ConfigureConventions(ModelConfigurationBuilder builder)
        {
            builder.Properties(typeof(Enum))
                .HaveConversion<string>()
                .HaveColumnType("nvarchar(32)");
        }

        public override int SaveChanges()
        {
            AddTimestamps();
            return base.SaveChanges();
        }

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken)
        {
            AddTimestamps();
            return await base.SaveChangesAsync(cancellationToken);
        }

        private void AddTimestamps()
        {
            var now = DateTime.UtcNow;

            foreach (var changedEntity in ChangeTracker.Entries())
            {
                if (changedEntity.Entity is Common entity)
                {
                    switch (changedEntity.State)
                    {
                        case EntityState.Added:
                            entity.CreatedDate = now;
                            entity.UpdatedDate = now;
                            break;

                        case EntityState.Modified:
                            Entry(entity).Property(x => x.CreatedDate).IsModified = false;
                            entity.UpdatedDate = now;
                            break;
                    }
                }
            }
        }
    }
}