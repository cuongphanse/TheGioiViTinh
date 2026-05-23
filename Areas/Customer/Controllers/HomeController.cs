using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Security.Claims;
using TheGioiViTinh.Models;
using TheGioiViTinh.Models.Entities;
using TheGioiViTinh.Models.Enum;
using TheGioiViTinh.Services.IServices;

namespace TheGioiViTinh.Areas.Customer.Controllers
{
	[Area("Customer")]
	public class HomeController : Controller
	{
		private readonly IProductService _productService;
		private readonly IShoppingCartService _shoppingCartService;
		public HomeController(IProductService productService, IShoppingCartService shoppingCartService)
		{
			_productService = productService;
			_shoppingCartService = shoppingCartService;
		}
		public async Task<IActionResult> Index()
		{
			var products = await _productService.GetAllAsync(
				includeProperties:
				"Category,Brand,ProductDiscounts.Discount");
			return View(products);
		}
		public async Task<IActionResult> Detail(Guid? id)
		{
			if(id == null)
			{
				return NotFound();
			}
			var product = await _productService.GetAsync(p=>p.Id == id, includeProperties: "Category,Brand,ProductImages,ProductDiscounts.Discount");
			//product?.ProductImages = product.ProductImages.OrderBy(pi => pi.Id).ToList();
			if (product == null)
			{
				return NotFound();
			}
			ShoppingCart cart = new()
			{
				Product = product,
				Count = 1,
				ProductId = product.Id
			};
			return View(cart);
		}
		[HttpPost]
		[Authorize]
		public async Task<IActionResult> Detail(ShoppingCart shoppingCart)
		{
			//if (!ModelState.IsValid)
			//{
			//	return View(shoppingCart);
			//}

			var claimsIdentity = (ClaimsIdentity)User.Identity!;
			var userId = claimsIdentity
				.FindFirst(ClaimTypes.NameIdentifier)?.Value;

			if (userId == null)
			{
				return Unauthorized();
			}

			shoppingCart.ApplicationUserId = userId;

			await _shoppingCartService.AddToCartAsync(shoppingCart);
			var count = await _shoppingCartService.GetCartCountAsync(userId);
			HttpContext.Session.SetInt32(Status.SessionCart, count);
			TempData["Success"] = "Thêm vào giỏ hàng thành công!";

			return RedirectToAction(
				"Detail",
				new { id = shoppingCart.ProductId }
			);
		}

		[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
		public IActionResult Error()
		{
			return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
		}
	}
}
