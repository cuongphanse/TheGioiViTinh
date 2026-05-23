namespace TheGioiViTinh.Models.VnPay
{
	public class VnPayRequestModel
	{
		public int OrderId { get; set; }
		public decimal Amount { get; set; }
		public DateTime CreatedDate { get; set; }
	}
}
