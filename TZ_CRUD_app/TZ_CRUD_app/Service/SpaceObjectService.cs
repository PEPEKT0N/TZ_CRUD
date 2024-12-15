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

        // получение космического объекта по id
        public async Task<SpaceObject?> GetAsync(int id)
        {
            return await _db.SpaceObjects.FirstOrDefaultAsync(spaceObject =>  spaceObject.Id == id);
        }

        // импортирование космического объекта

    }
}
