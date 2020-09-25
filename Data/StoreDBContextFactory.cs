using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Data
{
    public class StoreDBContextFactory: IDesignTimeDbContextFactory<StoreDbContext>
    {
        public StoreDbContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<StoreDbContext>();
            optionsBuilder.UseNpgsql("Host=127.0.0.1;Database=postgres;Username=postgres", options =>
            {
                options.MigrationsHistoryTable(HistoryRepository.DefaultTableName, "mhe");
                options.MigrationsAssembly("Data");
            });

            return new StoreDbContext(optionsBuilder.Options);
        }
    }
}