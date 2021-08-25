using MiniCRMCore.Areas.Common;

namespace MiniCRMCore.Areas.Email.Models
{
	public class EmailSettings : BaseEntity
	{
		public string SenderName { get; set; }
		public string SenderEmail { get; set; }
		public string SmtpHost { get; set; }
		public int Port { get; set; }
		public string Password { get; set; }

		public class Dto
		{
			public string SenderName { get; set; }
			public string SenderEmail { get; set; }
			public string SmtpHost { get; set; }
			public int Port { get; set; }
			public string Password { get; set; }
		}
	}
}