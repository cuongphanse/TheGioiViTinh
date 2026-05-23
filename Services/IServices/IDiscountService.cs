using TheGioiViTinh.Models.Entities;
using TheGioiViTinh.Models.ViewModels;

namespace TheGioiViTinh.Services.IServices
{
	public interface IDiscountService
	{
		Task CreateDiscountAsync(DiscountCreateVM vm);
		Task<IEnumerable<Discount>> GetAllAsync();

		Task DeleteAsync(Guid id);
	}
}
