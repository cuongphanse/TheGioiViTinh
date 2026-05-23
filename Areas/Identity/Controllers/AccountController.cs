using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Rendering;
using TheGioiViTinh.Models.Entities;
using TheGioiViTinh.Models.Enum;
using TheGioiViTinh.Models.ViewModels;
using TheGioiViTinh.Services.IServices;

namespace TheGioiViTinh.Areas.Identity.Controllers
{
	[Area("Identity")]
	public class AccountController : Controller
	{
		private readonly UserManager<ApplicationUser> _userManager;
		private readonly SignInManager<ApplicationUser> _signInManager;
		private readonly RoleManager<IdentityRole> _roleManager;
		private readonly IShoppingCartService _shoppingCartService;
		public AccountController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, RoleManager<IdentityRole> roleManager, IShoppingCartService shoppingCartService)
		{
			_userManager = userManager;
			_signInManager = signInManager;
			_roleManager = roleManager;
			_shoppingCartService = shoppingCartService;
		}
		public IActionResult Login(string? returnUrl = null)
		{
			ViewData["returnUrl"] = returnUrl;
			return View();
		}
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Login(LoginVM loginVM, string? returnUrl=null)
		{
			if (ModelState.IsValid)
			{
				var result = await _signInManager.PasswordSignInAsync(loginVM.Email, loginVM.Password, loginVM.RememberMe, lockoutOnFailure: false);
				if (result.Succeeded)
				{
					var user = await _userManager.FindByEmailAsync(loginVM.Email);
					if(user != null)
					{
						var count = await _shoppingCartService.GetCartCountAsync(user.Id);
						HttpContext.Session.SetInt32(Status.SessionCart, count);
					}
					TempData["Success"] = "Đăng nhập thành công!";
					if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
					{
						return Redirect(returnUrl);
					}
					if(User.IsInRole(UserRole.Admin) || User.IsInRole(UserRole.Employee))
					{
						return RedirectToAction("Index", "Dashboard", new { area = "Admin" });
					}
					else
					{					
						return RedirectToAction("Index", "Home", new { area = "Customer" });
					}
				}
				else
				{
					ModelState.AddModelError(string.Empty, "Email hoặc mật khẩu không đúng.");
					return View(loginVM);
				}
			}
			return View(loginVM);

		}
		public IActionResult Register(string? returnUrl = null)
		{
			var model = new RegisterVM();

			if(User.IsInRole(UserRole.Admin)){
				model.RoleList = [
					new SelectListItem { Value = UserRole.Customer, Text = "Khách hàng" },
					new SelectListItem { Value = UserRole.Employee, Text = "Nhân viên" },
					new SelectListItem { Value = UserRole.Admin, Text = "Quản trị viên" }
				];
			}
			else
			{
				model.RoleList = new List<SelectListItem>();
			}
			ViewData["returnUrl"] = returnUrl;
			return View(model);
		}
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Register(RegisterVM vm, string? returnUrl = null)
		{
			
			if (ModelState.IsValid)
			{
				var user = new ApplicationUser
				{
					UserName = vm.Email,
					Email = vm.Email,
					Name = vm.Name,
					StreetAddress = vm.StreetAddress,
					Ward = vm.Ward,
					District = vm.District,
					CityOrProvince = vm.CityOrProvince,
					PhoneNumber = vm.PhoneNumber
				};
				var result = await _userManager.CreateAsync(user,vm.Password);

				if (result.Succeeded)
				{
					if (User.IsInRole(UserRole.Admin) && !string.IsNullOrEmpty(vm.Role))
					{
						await _userManager.AddToRoleAsync(user, vm.Role);
						TempData["Success"] = $"Tài khoản '{user.Name}' đã được tạo với vai trò '{vm.Role}'.";
						return RedirectToAction("Index", "User", new { area = "Admin" });
					}
					else
					{
						await _userManager.AddToRoleAsync(user, UserRole.Customer);
						await _signInManager.SignInAsync(user, isPersistent: false);
						HttpContext.Session.SetInt32(Status.SessionCart, 0);
						TempData["Success"] = "Đăng ký thành công!";
						if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
						{
							return Redirect(returnUrl);
						}
						return RedirectToAction("Index", "Home", new { area = "Customer" });
					}
					
				}
				AddIdentityErrors(result);	
			}
			return View(vm);
		}
		public async Task<IActionResult> Logout()
		{
			await _signInManager.SignOutAsync();
			TempData["Success"] = "Đăng xuất thành công!";
			HttpContext.Session.SetInt32(Status.SessionCart, 0);
			return RedirectToAction("Index", "Home", new { area = "Customer" });
		}
		public IActionResult AccessDenied()
		{
			return View();
		}
		private void AddIdentityErrors(IdentityResult result)
		{
			var errorDictionary = new Dictionary<string, string>
	{
		{ "PasswordRequiresNonAlphanumeric", "Mật khẩu phải chứa ký tự đặc biệt." },
		{ "PasswordRequiresLower", "Mật khẩu phải chứa ít nhất 1 chữ thường." },
		{ "PasswordRequiresUpper", "Mật khẩu phải chứa ít nhất 1 chữ in hoa." },
		{ "PasswordRequiresDigit", "Mật khẩu phải chứa ít nhất 1 số." },
		{ "PasswordTooShort", "Mật khẩu quá ngắn." },
		{ "DuplicateUserName", "Tên đăng nhập đã tồn tại." },
		{ "DuplicateEmail", "Email đã được sử dụng." },
		{ "InvalidEmail", "Email không hợp lệ." }
	};

			foreach (var error in result.Errors)
			{
				var message = errorDictionary.ContainsKey(error.Code)
					? errorDictionary[error.Code]
					: error.Description;

				ModelState.AddModelError(string.Empty, message);
			}
		}
	}
}
