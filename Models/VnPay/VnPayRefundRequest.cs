namespace TheGioiViTinh.Models.VnPay
{
	public class VnPayRefundRequest
	{
		public string OrderId { get; set; } = null!;
		public long Amount { get; set; }
		public string TransactionDate { get; set; } = null!;
		public string CreateBy { get; set; } = null!;
	}
}
