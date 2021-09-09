using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;

namespace MiniCRMCore.Areas.Logs

{
	public class LogEntry
	{
		public Guid Id { get; set; }
		public DateTime Time { get; set; }
		public string Scope { get; set; }
		public LogLevel Level { get; set; }
		public LogEntryType Type { get; set; }
		public string Message { get; set; }
		public string FullMessage { get; set; }
	}

	public enum LogEntryType
	{
		Information,
		ClientError,
		ServerError,
		Security
	}
}
