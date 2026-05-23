using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace TheGioiViTinh.Models.Entities
{
	public class Category
	{
		[Key]
		public int Id { get; set; }

		[Required(ErrorMessage = "Tên danh mục không được để trống")]
		[DisplayName("Tên danh mục")]
		public string Name { get; set; } = string.Empty;

		[Required(ErrorMessage = "Thứ tự hiển thị không được để trống")]
		[Range(1, 100, ErrorMessage = "Thứ tự hiển thị phải từ 1-100")]
		[DisplayName("Thứ tự hiển thị")]
		public int? DisplayOrder { get; set; }
	}
}
