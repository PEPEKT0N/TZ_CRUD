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
        private readonly PagingService _pagination;
        public CategoryController(CategoryService categories, PagingService pagingService)
        {
            _categories = categories;
            _pagination = pagingService;
        }
        // GET /api/category - получение списка всех категорий
        [HttpGet]
        public async Task<List<CategoryMessage>> ListAllAsync([FromHeader(Name = "cursor")] string? cursor)
        {
            int id = _pagination.DecodeCursor(cursor);
            List<Category> categories;
            int? lastId;
            (categories, lastId) = await _categories.ListPageAsync(id, _pagination.PageSize);

            string? newCursor = "";
            if (categories.Count > 0)
            {
                newCursor = _pagination.EncodeCursor(lastId);
            }
            Response.Headers.Append("cursor", newCursor);
            
            return categories.Select(c => new CategoryMessage(
                Id: c.Id,
                Name: c.Name
                )).ToList();                
        }
        // GET /api/category/id - получение категории по id
        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetByIdAsync(int id)
        {
            Category? category = await _categories.GetAsync(id);
            if (category == null)
            {
                return NotFound(new ErrorMessage(Type: "CategoryNotFound",
                    Message: $"Category with id '{id}' not found"));
            }
            return Ok(category);
        }
        //POST /api/category - добавление категории в БД
        [HttpPost]
        public async Task<IActionResult> PostAsync(CategoryMessage categoryMessage)
        {
            if (await _categories.IsNameExists(categoryMessage.Name))
            {
                return Conflict(new ErrorMessage(Type: "DuplicatedCategoryName",
                    Message: $"Category with name '{categoryMessage.Name}' already exists"));
            }
            Category category = new Category() { Name = categoryMessage.Name };
            await _categories.AddAsync(category);
            return Created();
        }
        // DELETE /api/category - удаление категории из БД по id
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> DeleteAsync(int id)
        {
            if (await _categories.IsExists(id))
            {
                await _categories.DeleteAsync(id);
                return Ok();
            }
            return NotFound(new ErrorMessage(Type: "CategoryNotFound",
                Message: $"Category with id '{id}' not found"));
        }
        [HttpPatch]
        public async Task<IActionResult> UpdateAsync(Category category)
        {
            if (!await _categories.IsExists(category.Id))
            {
                return NotFound(new ErrorMessage(Type: "CategoryNotFound",
                Message: $"Category with id '{category.Id}' not found"));
            }
            await _categories.UpdateAsync(category);
            return Ok();
        }
    }
}
