using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using TheGioiViTinh.Models.Entities;
using TheGioiViTinh.Models.Enum;
using TheGioiViTinh.Models.ViewModels;
using TheGioiViTinh.Services.IServices;
using static System.Net.Mime.MediaTypeNames;

namespace TheGioiViTinh.Areas.Admin.Controllers
{
	[Area("Admin")]
	//[Authorize(Roles = UserRole.Admin)]
	public class ProductController : Controller
	{
		private readonly IProductService _productService;
		private readonly ICategoryService _categoryService;
		private readonly IBrandService _brandService;
		private readonly IWebHostEnvironment _webHostEnvironment;
		public ProductController(IProductService productService, ICategoryService categoryService, IBrandService brandService, IWebHostEnvironment webHostEnvironment)
		{
			_productService = productService;
			_categoryService = categoryService;
			_brandService = brandService;
			_webHostEnvironment = webHostEnvironment;
		}
		[AllowAnonymous]
		public IActionResult Index()
		{
			return View();
		}
		#region API CALLS
		[AllowAnonymous]
		[HttpGet]
		public async Task<IActionResult> GetAll()
		{
			var products =
				await _productService.GetAllProductsAsync();

			return Json(new { data = products });
		}

		#endregion
		public async Task<IActionResult> Create()
		{
			ProductCreateVM vm = new()
			{
				CategoryList = (await _categoryService
					.GetAllCategoriesAsync())
					.Select(c => new SelectListItem
					{
						Text = c.Name,
						Value = c.Id.ToString()
					}),

				BrandList = (await _brandService
					.GetAllBrandsAsync())
					.Select(b => new SelectListItem
					{
						Text = b.Name,
						Value = b.Id.ToString()
					})
			};

			return View(vm);
		}
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Create(ProductCreateVM vm)
		{
			if (!string.IsNullOrEmpty(vm.Product.Name)
				&& await _productService.IsProductNameExistsAsync(
					vm.Product.Name))
			{
				ModelState.AddModelError(
					"Product.Name",
					"Tên sản phẩm đã tồn tại!");
			}

			if (ModelState.IsValid)
			{
				await _productService.CreateProductAsync(vm);

				TempData["Success"] =
					"Thêm sản phẩm thành công!";

				return RedirectToAction(nameof(Index));
			}

			await LoadDropdownData(vm);

			return View(vm);
		}
		public async Task<IActionResult> Update(Guid id)
		{
			var product =
				await _productService.GetProductByIdAsync(id);

			if (product == null)
			{
				return NotFound();
			}

			ProductUpdateVM vm = new()
			{
				Product = product
			};

			await LoadDropdownData(vm);

			return View(vm);
		}
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Update(ProductUpdateVM vm)
		{
			ModelState.Remove("MainImage");
			if (!string.IsNullOrEmpty(vm.Product.Name)
				&& await _productService.IsProductNameExistsAsync(
					vm.Product.Name,
					vm.Product.Id))
			{
				ModelState.AddModelError(
					"Product.Name",
					"Tên sản phẩm đã tồn tại!");
			}

			if (ModelState.IsValid)
			{
				await _productService.UpdateProductAsync(vm);

				TempData["Success"] =
					"Cập nhật sản phẩm thành công!";

				return RedirectToAction(nameof(Index));
			}

			await LoadDropdownData(vm);

			return View(vm);
		}
		[HttpDelete]
		public async Task<IActionResult> Delete(Guid id)
		{
			try
			{
				await _productService
					.DeleteProductAsync(id);

				return Json(new
				{
					success = true,
					message = "Xóa sản phẩm thành công!"
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
		private async Task LoadDropdownData(ProductCreateVM vm)
		{
			vm.CategoryList =
				(await _categoryService.GetAllCategoriesAsync())
				.Select(c => new SelectListItem
				{
					Text = c.Name,
					Value = c.Id.ToString()
				});

			vm.BrandList =
				(await _brandService.GetAllBrandsAsync())
				.Select(b => new SelectListItem
				{
					Text = b.Name,
					Value = b.Id.ToString()
				});
		}
		private async Task LoadDropdownData(ProductUpdateVM vm)
		{
			vm.CategoryList =
				(await _categoryService.GetAllCategoriesAsync())
				.Select(c => new SelectListItem
				{
					Text = c.Name,
					Value = c.Id.ToString()
				});

			vm.BrandList =
				(await _brandService.GetAllBrandsAsync())
				.Select(b => new SelectListItem
				{
					Text = b.Name,
					Value = b.Id.ToString()
				});
		}
	}
}
