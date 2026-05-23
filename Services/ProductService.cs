using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using TheGioiViTinh.Data;
using TheGioiViTinh.Models.Entities;
using TheGioiViTinh.Models.ViewModels;
using TheGioiViTinh.Services.IServices;

namespace TheGioiViTinh.Services
{
	public class ProductService : IProductService
	{
		private readonly ApplicationDbContext _context;
		private readonly IFileService _fileService;
		public ProductService(ApplicationDbContext context, IFileService fileService)
		{
			_context = context;
			_fileService = fileService;
		}

		public async Task<IEnumerable<Product>> GetAllProductsAsync()
		{
			return await _context.Products
				.Include(p => p.Category)
				.Include(p => p.Brand)
				.ToListAsync();
		}
		public async Task<IEnumerable<Product>> GetAllAsync(
			Expression<Func<Product, bool>>? filter = null,
			string? includeProperties = null,
			bool tracked = false)
		{
			IQueryable<Product> query =
				tracked
				? _context.Products
				: _context.Products.AsNoTracking();

			// FILTER

			if (filter != null)
			{
				query = query.Where(filter);
			}

			// INCLUDE

			if (!string.IsNullOrEmpty(includeProperties))
			{
				foreach (var includeProp in includeProperties
					.Split(new char[] { ',' },
					StringSplitOptions.RemoveEmptyEntries))
				{
					query = query.Include(includeProp);
				}
			}

			return await query.ToListAsync();
		}
		public async Task<Product?> GetAsync(
			Expression<Func<Product, bool>> filter,
			string? includeProperties = null,
			bool tracked = false)
		{
			IQueryable<Product> query =
				tracked
				? _context.Products
				: _context.Products.AsNoTracking();

			query = query.Where(filter);

			// INCLUDE

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

		public async Task<Product?> GetProductByIdAsync(Guid? id)
		{
			return await _context.Products
				.Include(p => p.ProductImages)
				.FirstOrDefaultAsync(p => p.Id == id);
		}

		public async Task CreateProductAsync(ProductCreateVM vm)
		{
			string mainImageUrl = await _fileService.SaveImageAsync(vm.MainImage, "images/products");
			vm.Product.MainImageUrl = mainImageUrl;

			if(vm.SubImages != null && vm.SubImages.Any())
			{
				foreach(var image in vm.SubImages)
				{
					string imageUrl = await _fileService.SaveImageAsync(image, "images/products");
					vm.Product.ProductImages.Add(new ProductImage { ImageUrl = imageUrl , ProductId = vm.Product.Id });
					
				}
			}
			_context.Products.Add(vm.Product);

			await _context.SaveChangesAsync();
		}

		public async Task UpdateProductAsync(ProductUpdateVM vm)
		{
			var productFromDb = await _context.Products
				.Include(p => p.ProductImages)
				.FirstOrDefaultAsync(p => p.Id == vm.Product.Id);

			if (productFromDb == null)
			{
				throw new Exception("Không tìm thấy sản phẩm");
			}

			productFromDb.Name = vm.Product.Name;
			productFromDb.Description = vm.Product.Description;
			productFromDb.Price = vm.Product.Price;
			productFromDb.StockQuantity = vm.Product.StockQuantity;

			productFromDb.CPU = vm.Product.CPU;
			productFromDb.RAM = vm.Product.RAM;
			productFromDb.Storage = vm.Product.Storage;
			productFromDb.GPU = vm.Product.GPU;
			productFromDb.Color = vm.Product.Color;
			productFromDb.Weight = vm.Product.Weight;

			productFromDb.CategoryId = vm.Product.CategoryId;
			productFromDb.BrandId = vm.Product.BrandId;


			if (vm.MainImage != null)
			{
				// Xóa ảnh cũ

				if (!string.IsNullOrEmpty(productFromDb.MainImageUrl))
				{
					await _fileService.DeleteImageAsync(
						productFromDb.MainImageUrl);
				}

				// Upload ảnh mới

				string newMainImageUrl =
					await _fileService.SaveImageAsync(
						vm.MainImage,
						"images/products");

				productFromDb.MainImageUrl =
					newMainImageUrl;
			}


			if (vm.SubImages != null
				&& vm.SubImages.Any())
			{


				if (productFromDb.ProductImages != null
					&& productFromDb.ProductImages.Any())
				{
					foreach (var oldImage in productFromDb.ProductImages)
					{
						if (!string.IsNullOrEmpty(oldImage.ImageUrl))
						{
							await _fileService.DeleteImageAsync(
								oldImage.ImageUrl);
						}
					}
				}


				 _context.ProductImages.RemoveRange(
					productFromDb.ProductImages);


				foreach (var image in vm.SubImages)
				{
					string imageUrl =
						await _fileService.SaveImageAsync(
							image,
							"images/products");

					productFromDb.ProductImages.Add(
						new ProductImage
						{
							ImageUrl = imageUrl
						});
				}
			}

			await _context.SaveChangesAsync();
		}
		public async Task DeleteProductAsync(Guid? id)
		{
			var product = await _context.Products
				.Include(p => p.ProductImages)
				.FirstOrDefaultAsync(p => p.Id == id);

			if (product == null)
			{
				throw new Exception(
					"Không tìm thấy sản phẩm");
			}

			if (!string.IsNullOrEmpty(
				product.MainImageUrl))
			{
				await _fileService.DeleteImageAsync(
					product.MainImageUrl);
			}

			if (product.ProductImages != null
				&& product.ProductImages.Any())
			{
				foreach (var image in product.ProductImages)
				{
					if (!string.IsNullOrEmpty(
						image.ImageUrl))
					{
						await _fileService.DeleteImageAsync(
							image.ImageUrl);
					}
				}


				_context.ProductImages.RemoveRange(
					product.ProductImages);
			}


			_context.Products.Remove(product);

			await _context.SaveChangesAsync();
		}

		public async Task<bool> IsProductNameExistsAsync(string name, Guid? productId = null)
		{
			if (productId.HasValue)
			{
				return await _context.Products.AnyAsync(b =>
					b.Name.ToLower() == name.ToLower()
					&& b.Id != productId.Value);
			}

			return await _context.Products.AnyAsync(b =>
				b.Name.ToLower() == name.ToLower());
		}

	}
}
