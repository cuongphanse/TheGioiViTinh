using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TheGioiViTinh.Models.Entities
{
	public class ApplicationUser : IdentityUser 
	{
		[Required]
		public string Name { get; set; }
		[Display(Name = "Địa chỉ chi tiết (Số nhà, đường...)")]
		public string? StreetAddress { get; set; }

		[Display(Name = "Phường/Xã/Thị trấn")]
		public string? Ward { get; set; }

		[Display(Name = "Quận/Huyện/Thị xã")]
		public string? District { get; set; }

		[Display(Name = "Tỉnh/Thành phố")]
		public string? CityOrProvince { get; set; }

		[Display(Name = "Số điện thoại")]
		public string? PhoneNumber { get; set; }
		[NotMapped]
		public string Role { get; set; }

	}
}
