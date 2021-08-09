using System;
using System.Security.Claims;

namespace MiniCRMCore.Areas.Auth
{
	public static class ClaimsPrincipalExtensions
	{
		/// <summary>
		/// Получить идентификатор текущего пользователя.
		/// </summary>
		/// <param name="principal">Удостоверение пользователя</param>
		/// <returns>Идентификатор пользователя</returns>
		public static int GetUserId(this ClaimsPrincipal principal)
		{
			var identity = GetClaimsIdentity(principal);
			return ClaimsProvider.GetUserId(identity);
		}

		private static ClaimsIdentity GetClaimsIdentity(ClaimsPrincipal principal) => principal.Identity as ClaimsIdentity ?? throw new Exception("Invalid type of identity");
	}
}