using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using TheGioiViTinh.Models.ViewModels;
using TheGioiViTinh.Services.IServices;
using static System.Net.Mime.MediaTypeNames;

namespace TheGioiViTinh.Areas.Admin.Controllers
{
	[Area("Admin")]
	public class DiscountController : Controller
	{
		private readonly IDiscountService _discountService;
		private readonly IProductService _productService;
		public DiscountController(IDiscountService discountService, IProductService productService)
		{
			_discountService = discountService;
			_productService = productService;
		}
		public IActionResult Index()
		{
			return View();
		}

		[HttpGet]
		public async Task<IActionResult> GetAll()
		{
			var discounts = await _discountService
				.GetAllAsync();

			return Json(new
			{
				data = discounts
			});
		}

		[HttpDelete]
		public async Task<IActionResult> Delete(Guid id)
		{
			try
			{
				await _discountService
					.DeleteAsync(id);

				return Json(new
				{
					success = true,
					message = "Xóa khuyến mãi thành công!"
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
		public async Task<IActionResult> Create()
		{
			DiscountCreateVM vm = new()
			{
				Products = await _productService.GetAllAsync(
					includeProperties:
					"ProductDiscounts.Discount")
			};

			return View(vm);
		}
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Create(DiscountCreateVM vm)
		{
			if (ModelState.IsValid)
			{
				await _discountService.CreateDiscountAsync(vm);

				TempData["Success"] =
					"Tạo discount thành công!";

				return Redirect(nameof(Index));
			}

			vm.Products = await _productService.GetAllAsync(
				includeProperties:
				"ProductDiscounts.Discount");

			return View(vm);
		}
	}
	
}
