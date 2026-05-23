using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace TheGioiViTinh.Models.ViewModels
{
	public class RegisterVM
	{

		[Required(ErrorMessage = "Không được để trống!")]
		[EmailAddress]
		public string Email { get; set; }
		[Required(ErrorMessage = "Không được để trống!")]
		[DataType(DataType.Password)]
		[Display(Name = "Mật khẩu")]
		public string Password { get; set; }
		[Required(ErrorMessage = "Không được để trống!")]
		[DataType(DataType.Password)]
		[Display(Name = "Xác nhận mật khẩu")]
		[Compare("Password", ErrorMessage = "Mật khẩu xác nhận không khớp với mật khẩu mới.")]
		public string ConfirmPassword { get; set; }
		[Required]
		[Display(Name = "Họ và tên")]
		public string Name { get; set; } = string.Empty;
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

		public string? Role { get; set; } = string.Empty;

		[ValidateNever]
		public IEnumerable<SelectListItem>? RoleList { get; set; }

	}
}
