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
            base.OnConfiguring(optionsBuilder);
        }
    }
}
