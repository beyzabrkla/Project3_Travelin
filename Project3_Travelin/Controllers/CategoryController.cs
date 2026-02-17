using Microsoft.AspNetCore.Mvc;
using Project3_Travelin.DTOs.CategoryDTOs;
using Project3_Travelin.Services.CategoryServices;

namespace Project3_Travelin.Controllers
{
    public class CategoryController : Controller
    {
        private readonly ICategoryService _categoryService;

        public CategoryController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        public IActionResult CreateCategory()
        {
            return View();
        }

        [HttpPost]
        public async Task <IActionResult> CreateCategory(CreateCategoryDTO createCategoryDTO)
        {
            createCategoryDTO.IsStatus = true;
            await _categoryService.CreateCategoryAsync(createCategoryDTO);
            return RedirectToAction("CategoryList");
        }
    }
}
