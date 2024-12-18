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
            return await _db.Categories.OrderBy(c => c.Id).ToListAsync();
        }
        // получение категории объекта по id
        public async Task<Category?> GetAsync(int id)
        {
            return await _db.Categories.FirstOrDefaultAsync(category => category.Id == id);
        }
        public async Task<(List<Category>, int?)> ListPageAsync(int id, int limit)
        {
            var result = await _db.Categories
                .Where(c => c.Id > id)
                .Take(limit)
                .ToListAsync();
            int? lastId = result.LastOrDefault()?.Id;
            return (result, lastId);
        }
        
        // импортирование категории
        public async Task AddAsync(Category category)
        {
            await _db.Categories.AddAsync(category);
            await _db.SaveChangesAsync();
        }
        public async Task<bool> IsNameExists(string name)
        {
            return await _db.Categories.Where(c => c.Name == name).AnyAsync();
        }
        public async Task<bool> IsExists(int id)
        {
            return await _db.Categories.Where(c => c.Id == id).AnyAsync();
        }
        public async Task DeleteAsync(int id)
        {
            Category? category = await _db.Categories.FirstOrDefaultAsync(c => c.Id == id);
            if (category != null)
            {
                _db.Categories.Remove(category);
                await _db.SaveChangesAsync();
            }
        }
        public async Task UpdateAsync(Category category)
        {
            Category? updated = await _db.Categories.FirstOrDefaultAsync(c => c.Id == category.Id);
            updated!.Name = category.Name;            
            await _db.SaveChangesAsync();
        }
    }
}
