using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using TheGioiViTinh.Data;
using TheGioiViTinh.Models.Entities;
using TheGioiViTinh.Models.Enum;
using TheGioiViTinh.Services.IServices;

namespace TheGioiViTinh.Services
{
	public class OrderService : IOrderService
	{
		private readonly ApplicationDbContext _context;
		public OrderService(ApplicationDbContext context)
		{
			_context = context;
		}

		public async Task<OrderHeader> CreateOrderAsync(OrderHeader orderHeader)
		{
			_context.OrderHeaders.Add(orderHeader);
			await _context.SaveChangesAsync();
			return orderHeader;
		}

		public async Task<IEnumerable<OrderHeader>> GetAllOrderAsync(
			Expression<Func<OrderHeader, bool>>? filter = null,
			string? includeProperties = null,
			bool tracked = false, string? status= null)
		{
			IQueryable<OrderHeader> query =
				tracked
				? _context.OrderHeaders
				: _context.OrderHeaders.AsNoTracking();

			if (filter != null)
			{
				query = query.Where(filter);
			}

			if (!string.IsNullOrEmpty(includeProperties))
			{
				foreach (var includeProp in includeProperties
					.Split(new char[] { ',' },
					StringSplitOptions.RemoveEmptyEntries))
				{
					query = query.Include(includeProp);
				}
			}
			if(!string.IsNullOrEmpty(status) && status.ToLower() != "all")
			{
				query = query.Where(o => o.OrderStatus.ToLower() == status.ToLower());
			}
			return await query
				.OrderByDescending(u => u.OrderDate)
				.ToListAsync();
		}

		public async Task<OrderHeader?> GetOrderByIdAsync(
		Expression<Func<OrderHeader, bool>>? filter = null,
		string? includeProperties = null,
		bool tracked = false)
		{
			IQueryable<OrderHeader> query =
				tracked
				? _context.OrderHeaders
				: _context.OrderHeaders.AsNoTracking();

			if (filter != null)
			{
				query = query.Where(filter);
			}

			if (!string.IsNullOrEmpty(includeProperties))
			{
				foreach (var includeProp in includeProperties
					.Split(new char[] { ',' },
					StringSplitOptions.RemoveEmptyEntries))
				{
					query = query.Include(includeProp);
				}
			}

			return await query.FirstOrDefaultAsync();
		}

		public async Task UpdateOrderAsync(OrderHeader orderHeader)
		{
			_context.OrderHeaders.Update(orderHeader);
			await _context.SaveChangesAsync();
		}

		public async Task UpdateOrderStatusAsync(int id, string orderStatus, string? carrier = null, string? trackingNumber = null)
		{
			var order = await _context.OrderHeaders.FindAsync(id);
			if (order == null) { 
				throw new KeyNotFoundException($"Order {id} not found.");
			}
			order.OrderStatus = orderStatus;
			if(orderStatus == Status.StatusShipped)
			{
				order.ShippingDate = DateTime.Now;
				if (!string.IsNullOrEmpty(carrier))
				{
					order.Carrier = carrier;
				}
				if (!string.IsNullOrEmpty(trackingNumber))
				{
					order.TrackingNumber = trackingNumber;
				}					

			}
			await _context.SaveChangesAsync();
		}
	}
}
