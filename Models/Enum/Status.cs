namespace TheGioiViTinh.Models.Enum
{
	public static class Status
	{
		// ORDER STATUS

		public const string StatusPending = "Pending";
		public const string StatusApproved = "Approved";
		public const string StatusProcessing = "Processing";
		public const string StatusShipped = "Shipped";
		public const string StatusCompleted = "Completed";
		public const string StatusCancelled = "Cancelled";

		// PAYMENT STATUS

		public const string PaymentStatusPending = "Pending";
		public const string PaymentStatusApproved = "Approved";
		public const string PaymentStatusRejected = "Rejected";


		// PAYMENT METHOD

		public const string PaymentMethodCOD = "COD";
		public const string PaymentMethodVNPay = "VNPay";

		//Session
		public const string SessionCart = "TheGioiViTinhCart";
	}
}
