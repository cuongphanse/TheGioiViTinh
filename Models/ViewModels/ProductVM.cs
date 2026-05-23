using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;
using TheGioiViTinh.Models.Entities;

namespace TheGioiViTinh.Models.ViewModels
{
	public class ProductCreateVM
	{
		public Product Product { get; set; } = new();

		[Required(ErrorMessage = "Vui lòng chọn ảnh chính")]
		public IFormFile? MainImage { get; set; }

		public List<IFormFile>? SubImages { get; set; }

		[ValidateNever]
		public IEnumerable<SelectListItem>? CategoryList { get; set; }

		[ValidateNever]
		public IEnumerable<SelectListItem>? BrandList { get; set; }
	}
	public class ProductUpdateVM
	{
		public Product Product { get; set; } = new();

		public IFormFile? MainImage { get; set; }

		public List<IFormFile>? SubImages { get; set; }

		[ValidateNever]
		public IEnumerable<SelectListItem>? CategoryList { get; set; }

		[ValidateNever]
		public IEnumerable<SelectListItem>? BrandList { get; set; }
	}
}