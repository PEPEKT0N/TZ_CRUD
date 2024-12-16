using Microsoft.EntityFrameworkCore;

namespace TZ_CRUD_app.Model
{
    public class ApplicationDbContext : DbContext
    {
        // таблицы
        public required DbSet<SpaceObject> SpaceObjects { get; set; }
        public required DbSet<Category> Categories { get; set; }

        // конфигурирования
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            IConfigurationRoot config = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();
            string useConnection = config.GetSection("UseConnection").Value ?? "DefaultConnection";
            string? connectionString = config.GetConnectionString(useConnection);
            optionsBuilder.UseSqlServer(connectionString);
        }
    }
}
