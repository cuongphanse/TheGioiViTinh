using TheGioiViTinh.Models.Entities;

namespace TheGioiViTinh.Services.IServices
{
	public interface ICategoryService
	{
		Task<Category?> GetCategoryByIdAsync(int id);
		Task<IEnumerable<Category>> GetAllCategoriesAsync();
		Task<Category> CreateCategoryAsync(Category category);
		Task UpdateCategoryAsync(Category category);
		Task DeleteCategoryAsync(int id);
		Task<bool> IsCategoryNameExistsAsync(string name, int? categoryId = null);
	}
}
