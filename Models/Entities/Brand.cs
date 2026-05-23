using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace TheGioiViTinh.Models.Entities
{
	public class Brand
	{
		[Key]
		public int Id { get; set; }

		[Required(ErrorMessage = "Tên thương hiệu không được để trống")]
		[DisplayName("Tên thương hiệu")]
		public string Name { get; set; } = string.Empty;

		[ValidateNever]
		public ICollection<Product> Products { get; set; }
			= new List<Product>();
	}
}