using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TZ_CRUD_app.Model;
using TZ_CRUD_app.Service;

namespace TZ_CRUD_app.Api
{
    [Route("api/category")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly CategoryService _categories;
        public CategoryController(CategoryService categories)
        {
            _categories = categories;
        }

        [HttpGet]
        public async Task<List<CategoryItemMessage>> ListAllAsync()
        {
            List<Category> categories = await _categories.ListAllAsync();
            return categories
                .Select(c => new CategoryItemMessage(
                    Id: c.Id,
                    Name: c.Name
                    )).ToList();
        }
    }
}
