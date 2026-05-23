using Microsoft.EntityFrameworkCore;
using TheGioiViTinh.Data;
using TheGioiViTinh.Models.Entities;
using TheGioiViTinh.Services.IServices;

namespace TheGioiViTinh.Services
{
	public class CategoryService : ICategoryService
	{
		private readonly ApplicationDbContext _context;
		public CategoryService(ApplicationDbContext context)
		{
			_context = context;
		}
		public async Task<IEnumerable<Category>> GetAllCategoriesAsync()
		{
			return await _context.Categories.ToListAsync();
		}

		public async Task<Category?> GetCategoryByIdAsync(int id)
		{
			return await _context.Categories.FindAsync(id);
		}
		public async Task<Category> CreateCategoryAsync(Category category)
		{
			_context.Categories.Add(category);
			await _context.SaveChangesAsync();
			return category;
		}

		public async Task DeleteCategoryAsync(int id)
		{
			var category = _context.Categories.Find(id);
			if(category == null) {
				throw new KeyNotFoundException($"Danh mục {id} không tìm thấy");
			}
			_context.Categories.Remove(category);
			await _context.SaveChangesAsync();
		}

		public async Task UpdateCategoryAsync(Category category)
		{
			_context.Categories.Update(category);
			await _context.SaveChangesAsync();
		}

		public async Task<bool> IsCategoryNameExistsAsync(string name, int? categoryId = null)
		{
			if (categoryId.HasValue)
			{
				return await _context.Categories.AnyAsync(c => c.Name.ToLower() == name.ToLower() && c.Id != categoryId.Value);
			}
			else
			{
				return await _context.Categories.AnyAsync(c => c.Name.ToLower() == name.ToLower());
			}
		}
	}
}
