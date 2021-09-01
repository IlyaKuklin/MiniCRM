using MimeKit;
using MiniCRMCore.Areas.Email.Models;
using System.Linq;

namespace MiniCRMCore.Areas.Email
{
	public class EmailSenderService
	{
		private readonly ApplicationContext _context;

		public EmailSenderService(ApplicationContext context)
		{
			_context = context ?? throw new System.ArgumentNullException(nameof(context));
		}

		private EmailSettings _settings;

		private EmailSettings Settings
		{
			get
			{
				if (_settings == null)
				{
					_settings = _context.EmailSettings.First();
				}
				return _settings;
			}
		}

		public void SendEmail(string name, string email, string subject)
		{
			var message = new MimeMessage();
			message.From.Add(new MailboxAddress(this.Settings.SenderName, this.Settings.SenderEmail));
			message.To.Add(new MailboxAddress(name, email));

			message.Subject = subject;

			var builder = new BodyBuilder();
			builder.HtmlBody = "";
			message.Body = builder.ToMessageBody();

			using (var client = new MailKit.Net.Smtp.SmtpClient())
			{
				client.Connect(this.Settings.SmtpHost, this.Settings.Port, false);

				client.Authenticate(this.Settings.SenderEmail, this.Settings.Password);

				client.Send(message);

				client.Disconnect(true);
			}
		}
	}
}