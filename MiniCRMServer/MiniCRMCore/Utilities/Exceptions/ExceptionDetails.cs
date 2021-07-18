using Newtonsoft.Json;
using System;

namespace MiniCRMCore.Utilities.Exceptions
{
	/// <summary>
	/// Детали исключения.
	/// </summary>
	public class ExceptionDetails
	{
		/// <summary>
		/// Код ошибки.
		/// </summary>
		public int StatusCode { get; set; }

		/// <summary>
		/// Сообщение.
		/// </summary>
		public string Message { get; set; }

		/// <summary>
		/// ID ошибки в БД.
		/// </summary>
		public Guid Id { get; set; }

		public override string ToString()
		{
			return JsonConvert.SerializeObject(this);
		}
	}
}