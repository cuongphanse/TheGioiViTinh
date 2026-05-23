using TheGioiViTinh.Models.VnPay;

namespace TheGioiViTinh.Services.IServices
{
	public interface IVnPayService
	{
		string CreatePaymentUrl(HttpContext context, VnPayRequestModel model, string returnUrl);
		bool ValidateSignature(IQueryCollection collections);
		Task<string> Refund(VnPayRefundRequest request);
		bool ProcessPaymentResponse(IQueryCollection query, out int orderId);
	}
}
