using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Emit;
using TheGioiViTinh.Data;
using TheGioiViTinh.Models.Enum;
using TheGioiViTinh.Models.ViewModels;

namespace TheGioiViTinh.Areas.Admin.Controllers
{
	[Area("Admin")]
	public class DashboardController : Controller
	{
		private readonly ApplicationDbContext _db;
		public DashboardController(ApplicationDbContext db)
		{
			_db = db;
		}

		public async Task<IActionResult> Index()
		{
			var orders = await _db.OrderHeaders.ToListAsync();
			var productCount = await _db.Products.CountAsync();
			var usersCount = await _db.Users.CountAsync();

			DashboardVM dashboardVM = new()
			{
				TotalOrders = orders.Count,
				TotalProducts = productCount,
				TotalUsers = usersCount,
				TotalRevenue = orders.Where(o=>o.OrderStatus == Status.StatusApproved || o.OrderStatus == Status.StatusShipped).Sum(o=>o.OrderTotal)
			};
			return View(dashboardVM);
		}

		[HttpGet]
		public async Task<IActionResult> GetChartData()
		{
			var orders = await _db.OrderHeaders.ToListAsync();

			var now = DateTime.Now;

			var sixMonthsAgo = now.AddMonths(-5);

			// DOANH THU

			var monthlyRevenue = Enumerable.Range(0, 6).Select(i =>
			{
				var month = sixMonthsAgo.AddMonths(i);

				var revenue = orders
					.Where(o =>
						o.OrderDate.Year == month.Year &&
						o.OrderDate.Month == month.Month &&
						(o.OrderStatus == Status.StatusApproved ||
						 o.OrderStatus == Status.StatusShipped))
					.Sum(o => o.OrderTotal);

				return new
				{
					label = month.ToString("MM/yyyy"),
					revenue
				};

			}).ToList();

			// ĐƠN HÀNG

			var monthlyOrders = Enumerable.Range(0, 6).Select(i =>
			{
				var month = sixMonthsAgo.AddMonths(i);

				var count = orders.Count(o =>
					o.OrderDate.Year == month.Year &&
					o.OrderDate.Month == month.Month);

				return new
				{
					label = month.ToString("MM/yyyy"),
					count
				};

			}).ToList();

			return Json(new
			{
				monthlyRevenue,
				monthlyOrders
			});
		}
	}
}
