using Microsoft.AspNetCore.Mvc.Rendering;
using TheGioiViTinh.Models.Entities;

namespace TheGioiViTinh.Models.ViewModels
{
	public class DiscountCreateVM
	{
		public Discount Discount { get; set; } = new();

		public List<Guid> SelectedProductIds { get; set; }
			= new();

		public IEnumerable<Product> Products { get; set; }
			= new List<Product>();
	}
}