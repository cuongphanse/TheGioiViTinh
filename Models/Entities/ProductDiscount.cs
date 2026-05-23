using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace TheGioiViTinh.Models.Entities
{
	public class ProductDiscount
	{
		public Guid ProductId { get; set; } 

		[ValidateNever]
		public Product? Product { get; set; }

		public Guid DiscountId { get; set; } 

		[ValidateNever]
		public Discount? Discount { get; set; }
	}
}
