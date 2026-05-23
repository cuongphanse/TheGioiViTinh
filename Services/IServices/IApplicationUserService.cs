using TheGioiViTinh.Models.Entities;

namespace TheGioiViTinh.Services.IServices
{
	public interface IApplicationUserService
	{
		Task<ApplicationUser?> GetUserByIdAsync(string userId);
		Task<IEnumerable<ApplicationUser>> GetAllUsersAsync();
	}
}
