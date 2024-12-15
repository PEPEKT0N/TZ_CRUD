using Microsoft.EntityFrameworkCore;

namespace TZ_CRUD_app.Model
{
    // Category - класс, описывающий сущность категории
    [Index(nameof(Name), IsUnique = true)]
    public class Category
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;    // название категории
    }
}
