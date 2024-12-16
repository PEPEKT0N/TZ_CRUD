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
        [HttpGet]
        public async Task<List<SpaceObjectListItemMessage>> GetAllAsync([FromHeader(Name = "cursor")] string? cursor)
        {
            // List<SpaceObject> spaceObjects = await _spaceObjects.ListAllAsync();

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

            return spaceObjects.Select(spaceObject => new SpaceObjectListItemMessage(
                Id: spaceObject.Id,
                Name: spaceObject.Name,                
                DiscoveryYear: spaceObject.DiscoveryYear,
                Location: spaceObject.Location
                )).ToList();
        }

    }
}
