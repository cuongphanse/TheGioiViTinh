using Microsoft.EntityFrameworkCore;
using TheGioiViTinh.Data;
using TheGioiViTinh.Models.Entities;
using TheGioiViTinh.Models.ViewModels;
using TheGioiViTinh.Services.IServices;

namespace TheGioiViTinh.Services
{
	public class DiscountService : IDiscountService
	{
		private readonly ApplicationDbContext _context;
		public DiscountService(ApplicationDbContext context)
		{
			_context = context;
		}

		public async Task CreateDiscountAsync(
			DiscountCreateVM vm)
		{

			_context.Discounts.Add(vm.Discount);


			if (vm.SelectedProductIds != null
				&& vm.SelectedProductIds.Any())
			{
				foreach (var productId in vm.SelectedProductIds)
				{
					ProductDiscount productDiscount = new()
					{
						ProductId = productId,
						DiscountId = vm.Discount.Id
					};

					_context.ProductDiscounts
						.Add(productDiscount);
				}
			}

			await _context.SaveChangesAsync();
		}
		public async Task<IEnumerable<Discount>> GetAllAsync()
		{
			return await _context.Discounts
				.OrderByDescending(d => d.StartDate)
				.ToListAsync();
		}

		public async Task DeleteAsync(Guid id)
		{
			var discount = await _context.Discounts
				.FirstOrDefaultAsync(d => d.Id == id);

			if (discount == null)
			{
				throw new Exception("Không tìm thấy khuyến mãi");
			}

			_context.Discounts.Remove(discount);

			await _context.SaveChangesAsync();
		}
	}
}
