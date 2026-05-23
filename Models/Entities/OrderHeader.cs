using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TheGioiViTinh.Models.Entities
{
	public class OrderHeader
	{
		public int Id { get; set; }
		public string ApplicationUserId { get; set; } = string.Empty;
		[ForeignKey("ApplicationUserId")]
		[ValidateNever]
		public ApplicationUser ApplicationUser { get; set; }
		public DateTime OrderDate { get; set; } = DateTime.Now;
		public DateTime ShippingDate { get; set; }
		public decimal OrderTotal { get; set; }
		public string? OrderStatus { get; set; }
		[DisplayName("Hãng vận chuyển")]
		public string? Carrier { get; set; }

		[DisplayName("Mã vận đơn")]
		public string? TrackingNumber { get; set; }
		public string PaymentStatus { get; set; } = string.Empty;
		[DisplayName("Phương thức thanh toán")]
		public string PaymentMethod { get; set; } = string.Empty;
		[DisplayName("Mã giao dịch")]
		public string? TransactionId { get; set; }

		[DisplayName("Ngày thanh toán")]
		public DateTime? PaymentDate { get; set; }
		[Required]
		[DisplayName("Tên người nhận")]
		public string Name { get; set; } = string.Empty;

		[Required]
		[DisplayName("Số điện thoại")]
		public string PhoneNumber { get; set; } = string.Empty;

		[Required]
		[DisplayName("Địa chỉ")]
		public string StreetAddress { get; set; } = string.Empty;
		[Required]
		[DisplayName("Phường/Xã")]
		public string? Ward { get; set; }
		[Required]
		[DisplayName("Quận/Huyện")]
		public string? District { get; set; }
		[Required]
		[DisplayName("Tỉnh/Thành phố")]
		public string? CityOrProvince { get; set; }

		[DisplayName("Ghi chú")]
		public string? Note { get; set; }
		[ValidateNever]
		public IEnumerable<OrderDetails> OrderDetails { get; set; }
	}
}
