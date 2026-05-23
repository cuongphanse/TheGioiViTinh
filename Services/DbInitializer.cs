using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using TheGioiViTinh.Data;
using TheGioiViTinh.Models.Entities;
using TheGioiViTinh.Models.Enum;
using TheGioiViTinh.Services.IServices;

namespace TheGioiViTinh.Services
{
	public class DbInitializer : IDbInitializer
	{
		private readonly ApplicationDbContext _context;
		private readonly UserManager<ApplicationUser> _userManager;
		private readonly RoleManager<IdentityRole> _roleManager;
		public DbInitializer(ApplicationDbContext context, UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
		{
			_context = context;
			_userManager = userManager;
			_roleManager = roleManager;
		}
		public async Task InitializeAsync()
		{
			try
			{
				if ((await _context.Database.GetPendingMigrationsAsync()).Any())
				{
					await _context.Database.MigrateAsync();
				}
			}
			catch (Exception)
			{

				throw;
			}
			if (!await _roleManager.RoleExistsAsync(UserRole.Customer))
			{
				await _roleManager.CreateAsync(new IdentityRole(UserRole.Employee));
				await _roleManager.CreateAsync(new IdentityRole(UserRole.Admin));
				await _roleManager.CreateAsync(new IdentityRole(UserRole.Customer));
			}

			ApplicationUser user = await _userManager.FindByEmailAsync("admin@gmail.com");
			if (user == null)
			{
				var result = await _userManager.CreateAsync(new ApplicationUser
				{
					UserName = "admin@gmail.com",
					Email = "admin@gmail.com",
					EmailConfirmed = true,
					Name = "Quản trị viên",
					PhoneNumber = "034567890",
					StreetAddress ="302 Phan Huy Ích",
					District = "Tân Bình",
					Ward = "15",
					CityOrProvince = "Hồ Chí Minh",
				}, "Aa12345@");
				if (result.Succeeded)
				{
					user = await _userManager.FindByEmailAsync("admin@gmail.com");
					await _userManager.AddToRoleAsync(user, UserRole.Admin);
				}
			}
		}
	}
}
