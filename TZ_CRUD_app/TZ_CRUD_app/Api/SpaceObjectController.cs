using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TZ_CRUD_app.Model;
using TZ_CRUD_app.Service;

namespace TZ_CRUD_app.Api
{
    [Route("api/space-object")]
    [ApiController]
    public class SpaceObjectController : ControllerBase
    {
        private readonly SpaceObjectService _spaceObjects;
        private readonly CategoryService _Categories;
        private readonly PagingService _pagination;

        public SpaceObjectController(SpaceObjectService spaceObjects, CategoryService categories, PagingService pagingService)
        {
            _spaceObjects = spaceObjects;
            _Categories = categories;
            _pagination = pagingService;
        }
        // GET /api/space-object получение всех объектов из БД
        [HttpGet]
        public async Task<List<SpaceObjectListItemMessage>> GetAllAsync([FromHeader(Name = "cursor")] string? cursor)
        { 
            int id = _pagination.DecodeCursor(cursor);
            List<SpaceObject> spaceObjects;
            int? lastID;
            (spaceObjects, lastID) = await _spaceObjects.ListPageAsync(id, _pagination.PageSize);

            string? newCursor = "";
            if (spaceObjects.Count > 0)
            {
                newCursor = _pagination.EncodeCursor(lastID);
            }
            Response.Headers.Append("cursor", newCursor);

            return spaceObjects.Select(so => new SpaceObjectListItemMessage(
                Id: so.Id,
                Name: so.Name,                
                DiscoveryYear: so.DiscoveryYear,
                Location: so.Location
                )).ToList();
        }
        // GET /api/space-object/category/id - получение всех объектов одной категории
        [HttpGet("category/{id:int}")]
        public async Task<List<SpaceObjectListItemMessage>> GetAllByCategory([FromHeader(Name = "cursor")] string? cursor, int id)
        {            
            int cursorId = _pagination.DecodeCursor(cursor);
            List<SpaceObject> spaceObjects;
            int? lastID;
            (spaceObjects, lastID) = await _spaceObjects.ListCategoryObjectsPageAsync(id, cursorId, _pagination.PageSize);

            string? newCursor = "";
            if (spaceObjects.Count > 0)
            {
                newCursor = _pagination.EncodeCursor(lastID);
            }
            Response.Headers.Append("cursor", newCursor);

            return spaceObjects.Select(so => new SpaceObjectListItemMessage(
                Id: so.Id,
                Name: so.Name,
                DiscoveryYear: so.DiscoveryYear,
                Location: so.Location                              
                )).ToList();            
        }
        // GET /api/space-object/id - получение космического объекта по id
        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetByIdAsync(int id)
        {
            SpaceObject? spaceObject = await _spaceObjects.GetAsync(id);
            if (spaceObject == null)
            {
                return NotFound(new ErrorMessage(Type: "SpaceObjectNotFound", 
                    Message: $"Space object with id '{id}' not found"));
            }
            SpaceObjectMessage result = new SpaceObjectMessage(
                Id: spaceObject.Id,
                Name: spaceObject.Name,
                DiscoveryYear: spaceObject.DiscoveryYear,
                Location: spaceObject.Location,
                Categories: spaceObject.Categories!.Select(c => new CategoryMessage(Id: c.Id, Name: c.Name)).ToList()                
            );
            return Ok(result);
        }
        
        // POST /api/space-object - добавление космического объекта в БД
        // TODO: сделать проверку на уникальность названия объекта
        [HttpPost]
        public async Task<IActionResult> PostAsync(AddSpaceObjectMessage spaceObjectMessage)
        {
            if (await _spaceObjects.IsNameExists(spaceObjectMessage.Name))
            {
                return Conflict(new ErrorMessage(Type: "DuplicatedSpaceObjectName",
                    Message: $"Space object with name '{spaceObjectMessage.Name}' already exists"));
            }
            SpaceObject spaceObject = new SpaceObject()
            {
                Name = spaceObjectMessage.Name,
                DiscoveryYear = spaceObjectMessage.discoveryYear,
                Location = spaceObjectMessage.Location
            };
            await _spaceObjects.AddAsync(spaceObject);
            return Created();
        }
        // DELETE /api/space-object - удаление космического объекта по id
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> DeleteAsync(int id)
        {
            if (await _spaceObjects.IsExists(id))
            {
                await _spaceObjects.DeleteAsync(id);
                return NoContent();
            }
            return NotFound(new ErrorMessage(Type: "SpaceObjectNotFound", 
                Message: $"Space object with id '{id}' not found"));
        }
        // PATCH /api/space-object - обновление данных о космическом объекте
        [HttpPatch]
        public async Task<IActionResult> UpdateAsync(SpaceObject spaceObject)
        {
            if (!await _spaceObjects.IsExists(spaceObject.Id))
            {
                return NotFound(new ErrorMessage(Type: "SpaceObjectNotFound",
                    Message: $"Space object with id '{spaceObject.Id}' not found"));
            }
            await _spaceObjects.UpdateAsync(spaceObject);
            foreach(var c in spaceObject.Categories!)
            {
                await _spaceObjects.AddObjectToCategoryAsync(spaceObject.Id, c.Id);
            }                  
            return NoContent();
        }
    }
}
