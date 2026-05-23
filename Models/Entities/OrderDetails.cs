using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TheGioiViTinh.Models.Entities
{
	public class OrderDetails
	{
		public int Id { get; set; }
		[Required]
		public int OrderHeaderId { get; set; }
		[ForeignKey("OrderHeaderId")]
		[ValidateNever]
		public OrderHeader OrderHeader { get; set; }
		[Required]
		public Guid ProductId { get; set; }
		[ForeignKey("ProductId")]
		[ValidateNever]
		public Product Product { get; set; }
		public int Count { get; set; }
		public decimal Price { get; set; }
	}
}
