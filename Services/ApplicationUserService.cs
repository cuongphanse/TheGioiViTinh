using Microsoft.EntityFrameworkCore;
using TheGioiViTinh.Data;
using TheGioiViTinh.Models.Entities;
using TheGioiViTinh.Services.IServices;

namespace TheGioiViTinh.Services
{
	public class ApplicationUserService : IApplicationUserService
	{
		private readonly ApplicationDbContext _context;
		public ApplicationUserService(ApplicationDbContext context)
		{
			_context = context;
		}

		public async Task<IEnumerable<ApplicationUser>> GetAllUsersAsync()
		{
			return await _context.ApplicationUsers.ToListAsync();
		}

		public async Task<ApplicationUser?> GetUserByIdAsync(string userId)
		{
			return await _context.ApplicationUsers.FirstOrDefaultAsync(u=>u.Id == userId);
		}
	}
}
