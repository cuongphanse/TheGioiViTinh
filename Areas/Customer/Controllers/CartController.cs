using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using TheGioiViTinh.Models.Entities;
using TheGioiViTinh.Models.Enum;
using TheGioiViTinh.Models.ViewModels;
using TheGioiViTinh.Models.VnPay;
using TheGioiViTinh.Services.IServices;

namespace TheGioiViTinh.Areas.Customer.Controllers
{
	[Area("Customer")]
	[Authorize]
	public class CartController : Controller
	{
		private readonly IShoppingCartService _shoppingCartService;
		private readonly IOrderService _orderService;
		private readonly IApplicationUserService _userService;
		private readonly IVnPayService _vnPayService;
		public CartController(IShoppingCartService shoppingCartService, IOrderService orderService, IApplicationUserService userService, IVnPayService vnPayService)
		{
			_shoppingCartService = shoppingCartService;
			_orderService = orderService;
			_userService = userService;
			_vnPayService = vnPayService;
		}
		public async Task<IActionResult> Index()
		{
			var claimsIdentity = (ClaimsIdentity)User.Identity;
			var userId = claimsIdentity?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
			if (string.IsNullOrEmpty(userId))
			{
				return Unauthorized();
			}
			var cartItems = await _shoppingCartService.GetUserCartItemsAsync(userId);
			var user = await _userService.GetUserByIdAsync(userId);
			ShoppingCartVM shoppingCartVM = new()
			{
				ShoppingCartList = cartItems,
				OrderHeader = new()
			};
			shoppingCartVM.OrderHeader.ApplicationUser = user;
			shoppingCartVM.OrderHeader.ApplicationUserId = userId;
			shoppingCartVM.OrderHeader.Name = user.Name;
			shoppingCartVM.OrderHeader.PhoneNumber = user.PhoneNumber;
			shoppingCartVM.OrderHeader.StreetAddress = user.StreetAddress;
			shoppingCartVM.OrderHeader.District = user.District;
			shoppingCartVM.OrderHeader.CityOrProvince = user.CityOrProvince;
			shoppingCartVM.OrderHeader.Ward = user.Ward;

			foreach (var cart in shoppingCartVM.ShoppingCartList)
			{
				shoppingCartVM.OrderHeader.OrderTotal += cart.Price;
			}
			return View(shoppingCartVM);
		}
		[HttpPost]
		public async Task<IActionResult> Index(ShoppingCartVM shoppingCartVM)
		{
			try
			{
				var claimsIdentity = (ClaimsIdentity)User.Identity;

				var userId = claimsIdentity?
					.FindFirst(ClaimTypes.NameIdentifier)?.Value;

				if (string.IsNullOrEmpty(userId))
				{
					return Unauthorized();
				}

				var cartItems =
					await _shoppingCartService.GetUserCartItemsAsync(userId);

				if (!cartItems.Any())
				{
					TempData["Error"] = "Giỏ hàng đang trống";
					return RedirectToAction(nameof(Index));
				}

				shoppingCartVM.ShoppingCartList = cartItems;

				shoppingCartVM.OrderHeader.OrderDate = DateTime.Now;
				shoppingCartVM.OrderHeader.ApplicationUserId = userId;
				shoppingCartVM.OrderHeader.OrderStatus = Status.StatusApproved;

				foreach (var cart in cartItems)
				{
					shoppingCartVM.OrderHeader.OrderTotal += cart.Price;
				}

				shoppingCartVM.OrderHeader.OrderDetails =
					cartItems.Select(cart => new OrderDetails
					{
						ProductId = cart.ProductId,
						Price = cart.Price,
						Count = cart.Count
					}).ToList();
				shoppingCartVM.OrderHeader.PaymentMethod = shoppingCartVM.OrderHeader.PaymentMethod;
				await _orderService.CreateOrderAsync(shoppingCartVM.OrderHeader);

				if (shoppingCartVM.OrderHeader.PaymentMethod == Status.PaymentMethodVNPay)
				{
					var domain = $"{Request.Scheme}://{Request.Host}";

					var vnPayModel = new VnPayRequestModel
					{
						Amount = (decimal)shoppingCartVM.OrderHeader.OrderTotal,
						CreatedDate = DateTime.Now,
						OrderId = shoppingCartVM.OrderHeader.Id
					};

					string paymentUrl = _vnPayService.CreatePaymentUrl(
						HttpContext,
						vnPayModel,
						domain + Url.Action(
							"PaymentCallBack",
							"Cart",
							new { area = "Customer" })
					);

					return Redirect(paymentUrl);
				}

				await _shoppingCartService.ClearCartAsync(userId);

				await UpdateCartSession();

				TempData["Success"] = "Đặt hàng thành công";

				return RedirectToAction(
					"OrderConfirmation",
					new { id = shoppingCartVM.OrderHeader.Id });
			}
			catch (Exception)
			{
				TempData["Error"] =
					"Đã xảy ra lỗi khi đặt hàng";

				return RedirectToAction(nameof(Index));
			}
		}
		public async Task<IActionResult> PaymentCallBack()
		{
			var response = _vnPayService.ProcessPaymentResponse(
				Request.Query,
				out int orderId);

			var order = await _orderService.GetOrderByIdAsync(p=>p.Id == orderId);

			if (order == null)
			{
				TempData["Error"] = "Không tìm thấy đơn hàng";
				return RedirectToAction(nameof(Index));
			}

			if (response)
			{
				order.PaymentStatus = Status.PaymentStatusApproved;

				TempData["Success"] =
					"Thanh toán VNPay thành công";

				var claimsIdentity = (ClaimsIdentity)User.Identity;

				var userId = claimsIdentity?
					.FindFirst(ClaimTypes.NameIdentifier)?.Value;

				if (!string.IsNullOrEmpty(userId))
				{
					await _shoppingCartService.ClearCartAsync(userId);

					await UpdateCartSession();
				}

				return RedirectToAction(
					"OrderConfirmation",
					new { id = orderId });
			}

			order.PaymentStatus = Status.PaymentStatusRejected;

			TempData["Error"] =
				"Thanh toán VNPay thất bại";

			return RedirectToAction(nameof(Index));
		}
		public async Task<IActionResult> OrderConfirmation(int id)
		{
			
			return View(id);
		}
		public async Task<IActionResult> Plus(int cartId)
		{
			var cart = await _shoppingCartService.GetCartByIdAsync(cartId);
			if (cart != null)
			{
				if(cart.Count == 1000) 
				{

				}
				else
				{
					cart.Count++;
					await _shoppingCartService.UpdateCartAsync(cart);
					await UpdateCartSession();
				}
				
			}
			return RedirectToAction(nameof(Index));
		}
		public async Task<IActionResult> Minus(int cartId)
		{
			var cart = await _shoppingCartService.GetCartByIdAsync(cartId);
			if (cart != null)
			{
				cart.Count--;
				await _shoppingCartService.UpdateCartAsync(cart);
				await UpdateCartSession();
			}
			return RedirectToAction(nameof(Index));
		}
		public async Task<IActionResult> Remove(int cartId)
		{
			var cart = await _shoppingCartService.GetCartByIdAsync(cartId);
			if (cart != null)
			{
				cart.Count = 0;
				await _shoppingCartService.UpdateCartAsync(cart);
				await UpdateCartSession();
			}
			return RedirectToAction(nameof(Index));
		}
		[HttpPost]
		public async Task<IActionResult> UpdateCart(int cartId, int count)
		{
			var cart = await _shoppingCartService.GetCartByIdAsync(cartId);
			if(cart == null)
			{
				return NotFound();
			}
			if( count <= 0)
			{
				cart.Count = 0;
			}
			else
			{
				if (count < 1 || count > 1000)
				{
					return BadRequest(new
					{
						message = "Số lượng phải từ 1 đến 1000"
					});
				}
				else
				{
					cart.Count = count;
				}
			}
			cart.Count = count;
			await _shoppingCartService.UpdateCartAsync(cart);
			await UpdateCartSession();
			return Ok(new { succeeded = true });
		}
		private async Task UpdateCartSession()
		{
			var claimsIdentity = (ClaimsIdentity)User.Identity;
			var userId = claimsIdentity?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
			if (!string.IsNullOrEmpty(userId))
			{
				var count = await _shoppingCartService.GetCartCountAsync(userId);
				HttpContext.Session.SetInt32(Status.SessionCart, count);
			}
			
		}
	}
}