using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TheGioiViTinh.Models.Entities
{
	public class Product
	{
		[Key]
		public Guid Id { get; set; } = Guid.NewGuid();

		[Required(ErrorMessage = "Tên sản phẩm không được để trống")]
		[DisplayName("Tên sản phẩm")]
		public string Name { get; set; } = string.Empty;

		[DisplayName("Mô tả")]
		public string? Description { get; set; }

		[Required(ErrorMessage = "Giá bán không được để trống")]
		[Range(0, 999999999)]
		[DisplayName("Giá bán")]
		public decimal Price { get; set; }

		[Range(0, 99999, ErrorMessage ="Số lường từ 0 tới 99999")]
		[DisplayName("Số lượng tồn")]
		public int StockQuantity { get; set; }

		// THÔNG SỐ KỸ THUẬT

		[DisplayName("CPU")]
		public string? CPU { get; set; }

		[DisplayName("RAM")]
		public string? RAM { get; set; }

		[DisplayName("Ổ cứng")]
		public string? Storage { get; set; }

		[DisplayName("Card đồ họa")]
		public string? GPU { get; set; }
		[DisplayName("Màu sắc")]
		public string? Color { get; set; }

		[DisplayName("Cân nặng")]
		public string? Weight { get; set; }

		// CATEGORY

		[Required(ErrorMessage = "Vui lòng chọn danh mục")]
		[DisplayName("Danh mục")]
		public int CategoryId { get; set; }

		[ValidateNever]
		public Category? Category { get; set; }

		// BRAND

		[Required(ErrorMessage = "Vui lòng chọn thương hiệu")]
		[DisplayName("Thương hiệu")]
		public int BrandId { get; set; }

		[ValidateNever]
		public Brand Brand { get; set; }

		// HÌNH ẢNH
		[DisplayName("Ảnh chính")]
		public string MainImageUrl { get; set; } = string.Empty;

		[ValidateNever]
		public ICollection<ProductImage> ProductImages { get; set; }
			= new List<ProductImage>();

		[ValidateNever]
		public ICollection<ProductDiscount> ProductDiscounts { get; set; }
			= new List<ProductDiscount>();
		[NotMapped]
		public decimal FinalPrice
		{
			get
			{
				var now = DateTime.Now;

				var activeDiscount = ProductDiscounts
					.Where(pd => pd.Discount != null)
					.Select(pd => pd.Discount!)
					.FirstOrDefault(d =>
						d.IsActive &&
						d.StartDate <= now &&
						d.EndDate >= now);

				if (activeDiscount == null)
					return Price;

				return Price -
					(Price * activeDiscount.DiscountPercent / 100m);
			}
		}
	}
}