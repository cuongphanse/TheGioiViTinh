using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using TheGioiViTinh.Models.Entities;
using TheGioiViTinh.Models.Enum;
using TheGioiViTinh.Models.ViewModels;
using TheGioiViTinh.Services.IServices;

namespace TheGioiViTinh.Areas.Admin.Controllers
{
	[Area("Admin")]
	public class UserController : Controller
	{
		private readonly UserManager<ApplicationUser> _userManager;
		private readonly RoleManager<IdentityRole> _roleManager;
		private readonly IApplicationUserService _userService;
		public UserController(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager, IApplicationUserService userService)
		{
			_userManager = userManager;
			_roleManager = roleManager;
			_userService = userService;
		}
		[Authorize(Roles =UserRole.Admin)]
		public IActionResult Index()
		{
			return View();
		}

		[HttpPost]
		public async Task<IActionResult> LockUnlock([FromBody] string userId) 
		{
			var user = await _userService.GetUserByIdAsync(userId);
			if(user == null)
			{
				return Json(new { success = false, message = "Không tìm thấy người dùng" });
			}
			if(await _userManager.IsLockedOutAsync(user))
			{
				await _userManager.SetLockoutEndDateAsync(user, DateTimeOffset.UtcNow);
				return Json(new { success = true, message = "Tài khoản đã được mở khóa" });
			}
			else
			{
				await _userManager.SetLockoutEndDateAsync(user, DateTimeOffset.UtcNow.AddYears(100));
				return Json(new { success = true, message = "Tài khoản đã bị khóa" });
			} 
		}

		public async Task<IActionResult> RoleManagment(string userId) 
		{
			var user = await _userService.GetUserByIdAsync(userId);
			if (user == null)
			{
				return Json(new { success = false, message = "Không tìm thấy người dùng" });
			}
			RoleManagmentVM RoleVM = new()
			{
				ApplicationUser = user,
				RoleList = [
					new SelectListItem { Value = UserRole.Customer, Text = "Khách hàng" },
					new SelectListItem { Value = UserRole.Employee, Text = "Nhân viên" },
					new SelectListItem { Value = UserRole.Admin, Text = "Quản trị viên" }
				]
			};
			RoleVM.ApplicationUser.Role = (await _userManager.GetRolesAsync(user)).FirstOrDefault() ?? "No Role";

			return View(RoleVM);
		}
		[HttpPost]
		public async Task<IActionResult> RoleManagment(RoleManagmentVM roleVM)
		{
			var user = await _userService.GetUserByIdAsync(roleVM.ApplicationUser.Id);
			if (user == null)
			{
				return Json(new { success = false, message = "Không tìm thấy người dùng" });
			}

			string oldRole = (await _userManager.GetRolesAsync(user)).FirstOrDefault();
			if(!(roleVM.ApplicationUser.Role == oldRole))
			{
				await _userManager.RemoveFromRoleAsync(user, oldRole);
				await _userManager.AddToRoleAsync(user, roleVM.ApplicationUser.Role);
			}
			TempData["Success"] = "Cập nhật vai trò thành công";
			return RedirectToAction(nameof(Index));

		}
		public async Task<IActionResult> ChangePassword(string userId)
		{
			var user = await _userService.GetUserByIdAsync(userId);
			if (user == null)
			{
				return Json(new { success = false, message = "Không tìm thấy người dùng" });
			}
			AdminChangePasswordVM adminChangPwVM = new()
			{
				UserEmail = user.Email,
				UserId = user.Id,
				UserName = user.Name
			};
			

			return View(adminChangPwVM);
		}
		[HttpPost]
		public async Task<IActionResult> ChangePassword(AdminChangePasswordVM adminChangePasswordVM)
		{
			if(!ModelState.IsValid)
			{
				return View(adminChangePasswordVM);
			}
			var user = await _userService.GetUserByIdAsync(adminChangePasswordVM.UserId);
			if (user == null)
			{
				return Json(new { success = false, message = "Không tìm thấy người dùng" });
			}
			var token = await _userManager.GeneratePasswordResetTokenAsync(user);
			var result = await _userManager.ResetPasswordAsync(user, token,adminChangePasswordVM.NewPassword);
			if (result.Succeeded)
			{
				TempData["Success"] = "Đổi mật khẩu thành công";
				return RedirectToAction(nameof(Index));
			}
			foreach (var error in result.Errors)
			{
				ModelState.AddModelError(string.Empty, error.Description);
			}
			adminChangePasswordVM.UserEmail = user.Email;
			adminChangePasswordVM.UserName = user.Name;
			return View(adminChangePasswordVM);

		}
		#region Call API
		[AllowAnonymous]
		public async Task<IActionResult> GetAll()
		{
			var users = await _userService.GetAllUsersAsync();

			foreach(var user in users)
			{
				user.Role = (await _userManager.GetRolesAsync(user)).FirstOrDefault() ?? "No Role";
			}
			return Json(new {data = users});
		}
		#endregion
	}
}
