using Microsoft.Extensions.Configuration;
using MimeKit;
using MiniCRMCore.Areas.Email.Models;
using System;
using System.Linq;

namespace MiniCRMCore.Areas.Email
{
	public class EmailSenderService
	{
		private readonly ApplicationContext _context;
		private readonly IConfiguration _configuration;

		public EmailSenderService(ApplicationContext context,
			IConfiguration configuration)
		{
			_context = context ?? throw new ArgumentNullException(nameof(context));
			_configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
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

		public string BasePath
		{
			get
			{
				var section = _configuration.GetSection("BasePath");
				return section.Value;
			}
		}

		public void NotifyManager(string name, string email, string subject, int number, DateTime time)
		{
			var message = $"Клиент открыл коммерческое предложение №{number} (время: {time.ToString("dd.MM.yyyy")})";
			this.SendEmail(name, email, subject, message);
		}

		public void NotifyClient(string name, string email, string subject, string paramsString)
		{
			var message = $"Добрый день! Предлагаем вам ознакомиться с коммерческим предложением от нашей компании по <a href='{this.BasePath}offers/{paramsString}'>ссылке</a>. Будем рады обратной связи";
			this.SendEmail(name, email, subject, message);
		}

		public void SendEmail(string name, string email, string subject, string body)
		{
			var message = new MimeMessage();
			message.From.Add(new MailboxAddress(this.Settings.SenderName, this.Settings.SenderEmail));
			message.To.Add(new MailboxAddress(name, email));
			message.Subject = subject;

			//message.Body = new TextPart("plain")
			//{
			//	Text = body
			//};

			var builder = new BodyBuilder();
			builder.HtmlBody = body;
			message.Body = builder.ToMessageBody();

			using (var client = new MailKit.Net.Smtp.SmtpClient())
			{
				client.Connect(this.Settings.SmtpHost, this.Settings.Port, true);

				client.Authenticate(this.Settings.SenderEmail, this.Settings.Password);

				client.Send(message);

				client.Disconnect(true);
			}
		}
	}
}