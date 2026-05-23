using Microsoft.AspNetCore.Mvc;
using TheGioiViTinh.Models.Entities;
using TheGioiViTinh.Services.IServices;

namespace TheGioiViTinh.Areas.Admin.Controllers
{
	[Area("Admin")]
	public class BrandController : Controller
	{
		private readonly IBrandService _brandService;
		public BrandController(IBrandService brandService)
		{
			_brandService = brandService;
		}
		public async Task<IActionResult> Index()
		{
			var brands = await _brandService.GetAllBrandsAsync();
			return View(brands);
		}
		public IActionResult Create()
		{
			return View();
		}
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Create(Brand brand)
		{
			if (!string.IsNullOrEmpty(brand.Name)
				&& await _brandService.IsBrandNameExistsAsync(
					brand.Name))
			{
				ModelState.AddModelError(
					"Name",
					"Tên thương hiệu đã tồn tại!");
			}

			if (ModelState.IsValid)
			{
				await _brandService.CreateBrandAsync(brand);

				TempData["Success"] =
					"Thêm thương hiệu thành công!";

				return RedirectToAction("Index");
			}

			return View();
		}
		public async Task<IActionResult> Update(int? id)
		{
			if (id == null || id == 0)
			{
				return NotFound();
			}
			var brand = await _brandService.GetBrandByIdAsync(id.Value);
			if (brand == null)
			{
				return NotFound();
			}
			return View(brand);
		}
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Update(Brand brand)
		{
			if(!string.IsNullOrEmpty(brand.Name) && await _brandService.IsBrandNameExistsAsync(brand.Name, brand.Id))
			{
				ModelState.AddModelError("Name", "Tên thương hiệu đã tồn tại!");
			}
			if (ModelState.IsValid)
			{
				await _brandService.UpdateBrandAsync(brand);
				TempData["Success"] = "Cập nhật thương hiệu thành công!";
				return RedirectToAction("Index");
			}
			return View(brand);
		}
		public async Task<IActionResult> Delete(int? id)
		{
			if (id == null || id == 0)
			{
				return NotFound();
			}

			var brand = await _brandService
				.GetBrandByIdAsync(id.Value);

			if (brand == null)
			{
				return NotFound();
			}

			return View(brand);
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Delete(int id)
		{
			await _brandService.DeleteBrandAsync(id);

			TempData["Success"] =
				"Xóa thương hiệu thành công!";

			return RedirectToAction("Index");
		}
	}
}
