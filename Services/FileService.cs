using TheGioiViTinh.Services.IServices;

namespace TheGioiViTinh.Services
{
	public class FileService : IFileService
	{
		private readonly IWebHostEnvironment _webHostEnvironment;
		public FileService(IWebHostEnvironment webHostEnvironment)
		{
			_webHostEnvironment = webHostEnvironment;
		}	

		public async Task<string> SaveImageAsync(IFormFile file, string folder)
		{
			string wwwRootPath = _webHostEnvironment.WebRootPath;
			string uploadFolder = Path.Combine(wwwRootPath, folder);

			if (!Directory.Exists(uploadFolder))
			{
				Directory.CreateDirectory(uploadFolder);
			}

			string fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);

			string filePath = Path.Combine(uploadFolder, fileName);

			using (var stream =
				new FileStream(filePath, FileMode.Create))
			{
				await file.CopyToAsync(stream);
			}

			return "/" + folder.Replace("\\", "/") + "/" + fileName;
		}
		public async Task DeleteImageAsync(string imagePath)
		{
			if (string.IsNullOrEmpty(imagePath))
			{
				return;
			}

			string fullPath =
				Path.Combine(
					_webHostEnvironment.WebRootPath,
					imagePath.TrimStart('/'));

			if (File.Exists(fullPath))
			{
				await Task.Run(() => File.Delete(fullPath));
			}
		}
	}
}
