using System.Linq.Expressions;
using TheGioiViTinh.Models.Entities;

namespace TheGioiViTinh.Services.IServices
{
	public interface IOrderService
	{
		Task<OrderHeader> CreateOrderAsync(OrderHeader orderHeader);
		Task<OrderHeader?> GetOrderByIdAsync(
			Expression<Func<OrderHeader, bool>>? filter = null,
			string? includeProperties = null,
			bool tracked = false);
		Task<IEnumerable<OrderHeader>> GetAllOrderAsync(
			Expression<Func<OrderHeader, bool>>? filter = null,
			string? includeProperties = null,
			bool tracked = false, string? status =null);
		Task UpdateOrderAsync(OrderHeader orderHeader);
		Task UpdateOrderStatusAsync(int id, string orderStatus, string? carrier = null, string? trackingNumber = null);
	}
}
