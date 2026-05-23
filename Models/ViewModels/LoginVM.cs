using System.ComponentModel.DataAnnotations;

namespace TheGioiViTinh.Models.ViewModels
{
	public class LoginVM
	{
		[Required(ErrorMessage ="Không được để trống!")]
		[EmailAddress]
		public string Email { get; set; }
		[Required(ErrorMessage = "Không được để trống!")]
		[DataType(DataType.Password)]
		public string Password { get; set; }
		[Display(Name = "Ghi nhớ đăng nhập?")]
		public bool RememberMe { get; set; }
	}
}
