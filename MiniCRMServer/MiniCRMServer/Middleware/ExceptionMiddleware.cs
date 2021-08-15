using Microsoft.AspNetCore.Http;
using MiniCRMCore.Utilities.Exceptions;
using System;
using System.Net;
using System.Threading.Tasks;

namespace MiniCRMServer.Middleware
{
	public class ExceptionMiddleware
	{
		private readonly RequestDelegate _next;

		public ExceptionMiddleware(RequestDelegate next)
		{
			_next = next;
		}

		public async Task InvokeAsync(HttpContext httpContext)
		{
			try
			{
				await _next(httpContext);
			}
			catch (ApiException ex)
			{
				await HandleApiExceptionAsync(httpContext, ex);
			}
			catch (Exception ex)
			{
				await HandleExceptionAsync(httpContext, ex);
			}
		}

		private static Task HandleApiExceptionAsync(HttpContext context, ApiException exception)
		{
			context.Response.ContentType = "application/json";
			context.Response.StatusCode = exception.StatusCode;

			return context.Response.WriteAsync(new ExceptionDetails()
			{
				Id = Guid.NewGuid(),
				StatusCode = exception.StatusCode,
				Message = exception.Message
			}.ToString());
		}

		private static Task HandleExceptionAsync(HttpContext context, Exception exception)
		{
			context.Response.ContentType = "application/json";
			context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

			var message = exception.Message;
			if (exception.InnerException != null)
				message += $"\nInner:\n{exception.InnerException.Message}";

			return context.Response.WriteAsync(new ExceptionDetails
			{
				Id = Guid.NewGuid(),
				StatusCode = context.Response.StatusCode,
				Message = message
			}.ToString());
		}
	}
}
