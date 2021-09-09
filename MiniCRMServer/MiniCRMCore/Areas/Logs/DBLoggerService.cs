using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;

namespace MiniCRMCore.Areas.Logs
{
	public class DBLoggerService
	{
		private readonly ApplicationContext _context;

		public DBLoggerService(ApplicationContext context)
		{
			_context = context ?? throw new ArgumentNullException(nameof(context));
		}

		public Guid LogException(Exception exception, int statusCode = 500, string scope = null)
		{
			var type = statusCode >= 500
				? LogEntryType.ServerError
				: LogEntryType.ClientError;

			var logEntry = new LogEntry
			{
				Level = LogLevel.Error,
				Scope = scope,
				Type = type,
				Message = exception.Message,
				FullMessage = exception.ToString()
			};
			return this.Log(logEntry);
		}

		public Guid LogSecurityEvent(string ip, string message)
		{
			var logEntry = new LogEntry
			{
				Level = LogLevel.Information,
				Type = LogEntryType.Security,
				Message = message,
				FullMessage = $"IP: {ip}"
			};
			return this.Log(logEntry);
		}

		public Guid LogInformation(string body)
		{
			var logEntry = new LogEntry
			{
				Level = LogLevel.Information,
				Type = LogEntryType.Information,
				Message = body,
				FullMessage = body
			};
			return this.Log(logEntry);
		}

		private Guid Log(LogEntry logEntry)
		{
			logEntry.Time = DateTime.UtcNow;
			using (var transaction = _context.Database.BeginTransaction())
			{
				_context.LogEntries.Add(logEntry);
				transaction.Commit();
			}
			_context.SaveChanges();
			return logEntry.Id;
		}
	}
}
