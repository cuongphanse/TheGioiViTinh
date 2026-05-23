namespace TheGioiViTinh.Services.IServices
{
	public interface IFileService
	{
		Task<string> SaveImageAsync(IFormFile file, string folder);
		Task DeleteImageAsync(string imagePath);
	}
}
