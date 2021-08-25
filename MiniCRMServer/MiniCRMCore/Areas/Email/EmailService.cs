using MimeKit;

namespace MiniCRMCore.Areas.Email
{
	public class EmailService
	{
		public void SendEmail(string name, string email, string subject, string id)
		{
			var message = new MimeMessage();
			message.From.Add(new MailboxAddress("Prime Time", "robot@prime-time-online.ru"));
			message.To.Add(new MailboxAddress(name, email));

			message.Subject = subject;

			var builder = new BodyBuilder();

			builder.HtmlBody = ""
;

			message.Body = builder.ToMessageBody();

			using (var client = new MailKit.Net.Smtp.SmtpClient())
			{
				client.Connect("smtp.yandex.ru", 587, false);

				client.Authenticate("robot@prime-time-online.ru", "primetime499499");

				client.Send(message);

				client.Disconnect(true);
			}
		}
	}
}
