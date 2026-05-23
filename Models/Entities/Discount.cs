using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace TheGioiViTinh.Models.Entities
{
	public class Discount
	{
		[Key]
		public Guid Id { get; set; } = Guid.NewGuid();
		[Required]
		[DisplayName("Tên chương trình")]
		public string Name { get; set; } = string.Empty;

		[Range(0, 100)]
		[DisplayName("Phần trăm giảm")]
		public int DiscountPercent { get; set; }
		[DisplayName("Ngày bắt đầu")]
		public DateTime StartDate { get; set; } = DateTime.Now;

		[DisplayName("Ngày kết thúc")]
		public DateTime EndDate { get; set; } = DateTime.Now.AddDays(7);

		[DisplayName("Kích hoạt")]
		public bool IsActive { get; set; } = true;

		[ValidateNever]
		public ICollection<ProductDiscount> ProductDiscounts { get; set; }
			= new List<ProductDiscount>();
	}
}
