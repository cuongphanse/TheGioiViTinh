using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace TheGioiViTinh.Models.Entities
{
	public class ProductImage
	{
		[Key]
		public Guid Id { get; set; } = Guid.NewGuid();

		[Required]
		public string? ImageUrl { get; set; }

		public Guid ProductId { get; set; }

		[ValidateNever]
		public Product? Product { get; set; }
	}
}