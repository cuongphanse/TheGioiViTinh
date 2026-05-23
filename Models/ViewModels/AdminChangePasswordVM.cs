using System.ComponentModel.DataAnnotations;

namespace TheGioiViTinh.Models.ViewModels
{
	public class AdminChangePasswordVM
	{
		public string UserId { get; set; }
		[Display(Name = "User Email")]
		public string UserEmail { get; set; }
		[Display(Name = "Tên người dùng")]
		public string UserName { get; set; }
		[Required]
		[DataType(DataType.Password)]
		[Display(Name = "Mật khẩu mới")]
		public string NewPassword { get; set; }
		[DataType(DataType.Password)]
		[Display(Name = "Xác nhận mật khẩu mới")]
		[Compare("NewPassword", ErrorMessage = "Mật khẩu xác nhận không khớp với mật khẩu mới.")]
		public string ConfirmPassword { get; set; }
	}
}
