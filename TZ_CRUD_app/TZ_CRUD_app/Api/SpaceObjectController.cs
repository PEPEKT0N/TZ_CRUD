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

        public SpaceObjectController(SpaceObjectService spaceObjects, CategoryService categories)
        {
            _spaceObjects = spaceObjects;
            _Categories = categories;
        }
        [HttpGet]
        public async Task<List<SpaceObjectListItemMessage>> GetAllAsync()
        {
            List<SpaceObject> spaceObjects = await _spaceObjects.ListAllAsync();

            return spaceObjects.Select(spaceObject => new SpaceObjectListItemMessage(
                Id: spaceObject.Id,
                Name: spaceObject.Name,
                Type: spaceObject.Type,
                DiscoveryYear: spaceObject.DiscoveryYear,
                Location: spaceObject.Location
                )).ToList();
        }

    }
}
