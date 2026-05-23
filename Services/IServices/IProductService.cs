using System.Linq.Expressions;
using TheGioiViTinh.Models.Entities;
using TheGioiViTinh.Models.ViewModels;

namespace TheGioiViTinh.Services.IServices
{
	public interface IProductService
	{
		Task<IEnumerable<Product>> GetAllProductsAsync();
		Task<IEnumerable<Product>> GetAllAsync(Expression<Func<Product, bool>>? filter = null,
			string? includeProperties = null,
			bool tracked = false);
		Task<Product?> GetAsync(
			Expression<Func<Product, bool>> filter,
			string? includeProperties = null,
			bool tracked = false);
		Task<Product?> GetProductByIdAsync(Guid? id);

		Task CreateProductAsync(ProductCreateVM vm);
		Task UpdateProductAsync(ProductUpdateVM vm);

		Task DeleteProductAsync(Guid? id);

		Task<bool> IsProductNameExistsAsync(string name, Guid? productId = null);
	}
}
