using Microsoft.AspNetCore.Mvc;
using TheGioiViTinh.Data;
using TheGioiViTinh.Models.Entities;
using TheGioiViTinh.Services;
using TheGioiViTinh.Services.IServices;

namespace TheGioiViTinh.Areas.Admin.Controllers
{
	[Area("Admin")]
	public class CategoryController : Controller
	{
		private readonly ICategoryService _categoryService;
		public CategoryController(ICategoryService categoryService)
		{
			_categoryService = categoryService;
		}
		public async Task<IActionResult> Index()
		{
			var categories = await _categoryService.GetAllCategoriesAsync();
			return View(categories);
		}
		
		public IActionResult Create()
		{
			return View();
		}
		[HttpPost]
		[ValidateAntiForgeryToken]
		//[ActionName("Create")]
		public async Task<IActionResult> Create(Category category)
		{
			if (!string.IsNullOrEmpty(category.Name) && await _categoryService.IsCategoryNameExistsAsync(category.Name))
			{
				ModelState.AddModelError("Name", "Tên danh mục đã tồn tại!");
			}
			if (ModelState.IsValid)
			{				
				await _categoryService.CreateCategoryAsync(category);
				TempData["Success"] = "Thêm danh mục thành công!";
				return RedirectToAction("Index");
			}
			return View();
		}
		public async Task<IActionResult> Update(int? id)
		{
			if(id == null || id == 0)
			{
				return NotFound();
			}
			var category = await _categoryService.GetCategoryByIdAsync(id.Value);
			if (category == null)
			{
				return NotFound();
			}
			return View(category);
		}
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Update(Category category)
		{
			if (!string.IsNullOrEmpty(category.Name) && 
				await _categoryService.IsCategoryNameExistsAsync(category.Name,category.Id))
			{
				ModelState.AddModelError("Name", "Tên danh mục đã tồn tại!");
			}
			if (ModelState.IsValid)
			{
				await _categoryService.UpdateCategoryAsync(category);
				TempData["Success"] = "Thay đổi danh mục thành công!";
				return RedirectToAction("Index");
			}
			return View();
		}
		[HttpDelete]
		public async Task<IActionResult> Delete(int id)
		{
			try
			{
				await _categoryService.DeleteCategoryAsync(id);
				return Json(new
				{
					success = true,
					message = "Xóa danh mục thành công!"
				});
			}
			catch (Exception ex)
			{
				return Json(new
				{
					success = false,
					message = ex.Message
				});
			}

		}
		#region API CALLS
		public async Task<IActionResult> GetAll()
		{
			var categories = await _categoryService.GetAllCategoriesAsync();
			return Json(new { data = categories });
		}
		#endregion
	}
}
