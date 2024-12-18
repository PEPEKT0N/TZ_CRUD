using Microsoft.EntityFrameworkCore;
using TZ_CRUD_app.Model;

namespace TZ_CRUD_app.Service
{
    // SpaceObjectService - класс для выполнения CRUD - операций с космическими объектами
    public class SpaceObjectService
    {
        private readonly ApplicationDbContext _db;
        public SpaceObjectService(ApplicationDbContext db)
        {
            _db = db;
        }
        // получения списка всех космических объектов
        public async Task<List<SpaceObject>> ListAllAsync()
        {
            return await _db.SpaceObjects.ToListAsync();
        }

        public async Task<(List<SpaceObject>, int?)> ListPageAsync(int id, int limit)
        {
            var result = await _db.SpaceObjects
                .Where(so => so.Id > id)
                .Take(limit)
                .ToListAsync();
            int? lastId = result.LastOrDefault()?.Id;
            return (result, lastId);
        }
        public async Task<(List<SpaceObject>, int?)> ListCategoryObjectsPageAsync(int categoryId, int cursorId, int limit)
        {
            List<SpaceObject> result = await _db.SpaceObjects
                .Where(so => so.Categories!.Any(c => c.Id == categoryId) && so.Id > cursorId)
                .OrderBy(so => so.Id)
                .Take(limit)
                .ToListAsync();
            int? lastId = result.LastOrDefault()?.Id;
            return (result, lastId);
        }

        // получение космического объекта по id
        public async Task<SpaceObject?> GetAsync(int id)
        {
            return await _db.SpaceObjects.Include(so => so.Categories).FirstOrDefaultAsync(so => so.Id == id);
        }

        // импортирование космического объекта
        public async Task AddAsync(SpaceObject spaceObject)
        {
            await _db.SpaceObjects.AddAsync(spaceObject);
            await _db.SaveChangesAsync();
        }
        public async Task<bool> IsExists(int id)
        {
            return await _db.SpaceObjects.Where(so => so.Id == id).AnyAsync();
        }
        public async Task<bool> IsNameExists(string Name)
        {
            return await _db.SpaceObjects.Where(so => so.Name == Name).AnyAsync();
        }
        public async Task DeleteAsync(int id)
        {
            SpaceObject? spaceObject = await _db.SpaceObjects.FirstOrDefaultAsync(so => so.Id == id);
            if (spaceObject != null)
            {
                _db.SpaceObjects.Remove(spaceObject);
                await _db.SaveChangesAsync();
            }
        }
        public async Task UpdateAsync(SpaceObject spaceObject)
        {
            SpaceObject? updated = await _db.SpaceObjects.FirstOrDefaultAsync(so => so.Id == spaceObject.Id);
            updated!.Name = spaceObject.Name;
            updated!.DiscoveryYear = spaceObject.DiscoveryYear;
            updated!.Location = spaceObject.Location;
            await _db.SaveChangesAsync();
        }
        public async Task AddObjectToCategoryAsync(int spaceObjectId, int categoryId)
        {
            SpaceObject? updated = await _db.SpaceObjects
                .Include(so => so.Categories)
                .FirstOrDefaultAsync(so => so.Id == spaceObjectId);
            Category? newCategory = await _db.Categories.FirstOrDefaultAsync(c => c.Id == categoryId);
            if (newCategory != null)
            {
                updated!.Categories!.Add(newCategory!);
                await _db.SaveChangesAsync();
            }
        }
        public async Task DeleteByIdCategory(int spaceObjectId, int categoryId)
        {
            SpaceObject? updated = await _db.SpaceObjects
                .Include(so => so.Categories)
                .FirstOrDefaultAsync(so => so.Id == spaceObjectId);
            Category? removedCategory = await _db.Categories.FirstOrDefaultAsync(c => c.Id == categoryId);
            if (removedCategory != null)
            {
                updated!.Categories!.Remove(removedCategory!);
                await _db.SaveChangesAsync();
            }

        }
    }
}
