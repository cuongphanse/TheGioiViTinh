using TheGioiViTinh.Models.Entities;

namespace TheGioiViTinh.Services.IServices
{
	public interface IBrandService
	{
		Task<Brand?> GetBrandByIdAsync(int id);

		Task<IEnumerable<Brand>> GetAllBrandsAsync();

		Task<Brand> CreateBrandAsync(Brand brand);

		Task UpdateBrandAsync(Brand brand);

		Task DeleteBrandAsync(int id);

		Task<bool> IsBrandNameExistsAsync(string name, int? brandId = null);
	}
}
