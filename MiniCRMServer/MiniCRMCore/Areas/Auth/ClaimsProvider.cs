using System.Linq;
using System.Security.Claims;

namespace MiniCRMCore.Areas.Auth
{
	public class ClaimsProvider
	{
		public class Consts
		{
			public const string USER_ID = "user_id";
			public const string LOGIN = "login";
			public const string ROLE = "role";
		}

		/// <summary>
		/// Получение значение утверждения
		/// </summary>
		/// <param name="identity">Пользователь</param>
		/// <param name="claimType">Ключ утверждения</param>
		/// <returns>Значение утвержедния</returns>
		private static string GetClaimValue(ClaimsIdentity identity, string claimType)
		{
			var claim = identity.Claims.FirstOrDefault(x => x.Type == claimType);
			if (claim == null)
				return null;
			return claim.Value;
		}

		/// <summary>
		/// Получить идентификатор пользователя
		/// </summary>
		/// <param name="identity">Пользователь</param>
		/// <returns>Идентификатор</returns>
		public static int GetUserId(ClaimsIdentity identity)
		{
			var claimValue = GetClaimValue(identity, Consts.USER_ID);
			return string.IsNullOrEmpty(claimValue) ? 0 : int.Parse(claimValue);
		}

		public static string GetUserLogin(ClaimsIdentity identity)
		{
			var claimValue = GetClaimValue(identity, Consts.LOGIN);
			return string.IsNullOrEmpty(claimValue) ? null : claimValue;
		}

		public static string GetUserRole(ClaimsIdentity identity)
		{
			var claimValue = GetClaimValue(identity, Consts.ROLE);
			return string.IsNullOrEmpty(claimValue) ? null : claimValue;
		}
	}
}