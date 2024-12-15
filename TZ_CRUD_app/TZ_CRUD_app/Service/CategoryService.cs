using Microsoft.EntityFrameworkCore;
using TZ_CRUD_app.Model;

namespace TZ_CRUD_app.Service
{
    // CategoryService - класс для выполнения операций со категориями
    public class CategoryService
    {
        private readonly ApplicationDbContext _db;
        public CategoryService(ApplicationDbContext db)
        {
            _db = db;
        }

        // получение списка всех категорий 
        public async Task<List<Category>> ListAllAsync()
        {
            return await _db.Categories.ToListAsync();
        }
        // получение категории объекта по id
        public async Task<Category?> GetAsync(int id)
        {
            return await _db.Categories.FirstOrDefaultAsync(category => category.Id == id);
        }

        // импортирование категории
    }
}
