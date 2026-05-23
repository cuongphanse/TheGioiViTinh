using Microsoft.AspNetCore.Mvc.Rendering;
using TheGioiViTinh.Models.Entities;

namespace TheGioiViTinh.Models.ViewModels
{
	public class RoleManagmentVM
	{
		public ApplicationUser ApplicationUser { get; set; }
		public IEnumerable<SelectListItem> RoleList { get; set; }
	}
}
