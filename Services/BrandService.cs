using Microsoft.EntityFrameworkCore;
using TheGioiViTinh.Data;
using TheGioiViTinh.Models.Entities;
using TheGioiViTinh.Services.IServices;

namespace TheGioiViTinh.Services
{
	public class BrandService : IBrandService
	{
		private readonly ApplicationDbContext _context;

		public BrandService(ApplicationDbContext context)
		{
			_context = context;
		}

		public async Task<IEnumerable<Brand>> GetAllBrandsAsync()
		{
			return await _context.Brands.ToListAsync();
		}

		public async Task<Brand?> GetBrandByIdAsync(int id)
		{
			return await _context.Brands.FindAsync(id);
		}

		public async Task<Brand> CreateBrandAsync(Brand brand)
		{
			_context.Brands.Add(brand);

			await _context.SaveChangesAsync();

			return brand;
		}

		public async Task UpdateBrandAsync(Brand brand)
		{
			_context.Brands.Update(brand);

			await _context.SaveChangesAsync();
		}

		public async Task DeleteBrandAsync(int id)
		{
			var brand = await _context.Brands.FindAsync(id);

			if (brand == null)
			{
				throw new KeyNotFoundException(
					$"Thương hiệu {id} không tìm thấy");
			}

			_context.Brands.Remove(brand);

			await _context.SaveChangesAsync();
		}

		public async Task<bool> IsBrandNameExistsAsync(
			string name,
			int? brandId = null)
		{
			if (brandId.HasValue)
			{
				return await _context.Brands.AnyAsync(b =>
					b.Name.ToLower() == name.ToLower()
					&& b.Id != brandId.Value);
			}

			return await _context.Brands.AnyAsync(b =>
				b.Name.ToLower() == name.ToLower());
		}
	}
}