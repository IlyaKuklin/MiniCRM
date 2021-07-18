using System;

namespace MiniCRMCore.Utilities.Exceptions
{
	/// <summary>
	/// Серверная ошибка.
	/// </summary>
	public class ApiException : Exception
	{
		public int StatusCode { get; }

		/// <summary>
		/// Серверная ошибка.
		/// </summary>
		/// <param name="message"></param>
		/// <param name="statusCode"></param>
		public ApiException(string message, int statusCode = 500) : base(message)
		{
			this.StatusCode = statusCode;
		}

		public class Dto
		{
			public Guid Id { get; set; }
			public string Message { get; set; }
		}
	}
}