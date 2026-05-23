using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Security.Claims;
using TheGioiViTinh.Models.Entities;
using TheGioiViTinh.Models.Enum;
using TheGioiViTinh.Models.ViewModels;
using TheGioiViTinh.Services.IServices;
using static System.Net.Mime.MediaTypeNames;

namespace TheGioiViTinh.Areas.Admin.Controllers
{
	[Area("Admin")]
	[Authorize(Roles = UserRole.Admin)]
	public class OrderController : Controller
	{
		private readonly IOrderService _orderService;
		[BindProperty]
		public OrderHeader OrderHeader { get; set; }
		public OrderController(IOrderService orderService)
		{
			_orderService = orderService;
		}
		[AllowAnonymous]
		public IActionResult Index()
		{
			return View();
		}
		[AllowAnonymous]
		public async Task<IActionResult> Details(int orderId)
		{
			OrderHeader = await _orderService.GetOrderByIdAsync(
				p => p.Id == orderId,
				includeProperties: "ApplicationUser,OrderDetails.Product.ProductDiscounts.Discount"
			);

			if (OrderHeader == null)
			{
				return NotFound();
			}

			return View(OrderHeader);
		}
		[HttpPost]
		[Authorize(Roles = UserRole.Admin + "," + UserRole.Employee)]
		public async Task<IActionResult> UpdateOrderDetails()
		{
			var orderHeaderFromDb = await _orderService.GetOrderByIdAsync(
				p => p.Id == OrderHeader.Id);
			orderHeaderFromDb.Name = OrderHeader.Name;
			orderHeaderFromDb.PhoneNumber = OrderHeader.PhoneNumber;
			orderHeaderFromDb.StreetAddress = OrderHeader.StreetAddress;
			orderHeaderFromDb.Ward = OrderHeader.Ward;
			orderHeaderFromDb.District = OrderHeader.District;
			orderHeaderFromDb.CityOrProvince = OrderHeader.CityOrProvince;
			if (!string.IsNullOrEmpty(OrderHeader.Carrier) && OrderHeader.OrderStatus == Status.StatusShipped)
			{
				orderHeaderFromDb.Carrier = OrderHeader.Carrier;
			}
			if (!string.IsNullOrEmpty(OrderHeader.TrackingNumber) && OrderHeader.OrderStatus == Status.StatusShipped)
			{
				orderHeaderFromDb.TrackingNumber = OrderHeader.TrackingNumber;
			}
			await _orderService.UpdateOrderAsync(orderHeaderFromDb);
			TempData["Success"] = "Cập nhật đơn hàng thành công";
			return RedirectToAction("Details", new { orderId = OrderHeader.Id });
		}
		[HttpPost]
		[Authorize(Roles = UserRole.Admin + "," + UserRole.Employee)]
		public async Task<IActionResult> UpdateOrderStatus(string status)
		{
			var orderHeader = await _orderService.GetOrderByIdAsync(p => p.Id == OrderHeader.Id);
			if (orderHeader == null) { 
				TempData["Error"] = "Đơn hàng không tồn tại";
				return RedirectToAction(nameof(Index));
			}
			string successMessage;
			switch (status)
			{
				case Status.StatusProcessing:
					await _orderService.UpdateOrderStatusAsync(OrderHeader.Id, status);
					successMessage = "Đơn hàng đang được xử lý";
					break;
				case Status.StatusShipped:
					if(string.IsNullOrEmpty(OrderHeader.Carrier) || string.IsNullOrEmpty(OrderHeader.TrackingNumber))
					{
						TempData["Error"] = "Vui lòng cung cấp thông tin vận chuyển trước khi cập nhật trạng thái giao hàng";
						return RedirectToAction("Details", new { orderId = OrderHeader.Id });
					}
					await _orderService.UpdateOrderStatusAsync(OrderHeader.Id, Status.StatusShipped,OrderHeader.Carrier,OrderHeader.TrackingNumber);
					successMessage = "Đơn hàng đã được giao";
					break;
				case Status.StatusCancelled:
					await _orderService.UpdateOrderStatusAsync(OrderHeader.Id, status);
					successMessage = "Đơn hàng đã bị hủy";
					break;
				default:
					TempData["Error"] = "Trạng thái đơn hàng không hợp lệ";
					return RedirectToAction("Details", new { orderId = OrderHeader.Id });
			}
			TempData["Success"] = successMessage;
			return RedirectToAction("Details", new { orderId = OrderHeader.Id });
		}
		#region API CALLS
		[AllowAnonymous]
		[HttpGet]
		public async Task<IActionResult> GetAll(string? status)
		{
			IEnumerable<OrderHeader> orders;

			if (User.IsInRole(UserRole.Admin) || User.IsInRole(UserRole.Employee))
			{
				orders = await _orderService.GetAllOrderAsync(
					includeProperties: "ApplicationUser",
					status: status
				);
			}
			else
			{
				var claimsIdentity = (ClaimsIdentity)User.Identity;

				var userId = claimsIdentity?
					.FindFirst(ClaimTypes.NameIdentifier)?.Value;

				if (string.IsNullOrEmpty(userId))
				{
					return Unauthorized();
				}

				orders = await _orderService.GetAllOrderAsync(
					u => u.ApplicationUserId == userId,
					includeProperties: "ApplicationUser",
					status: status
				);
			}

			return Json(new { data = orders });
		}

		#endregion

	}
}
