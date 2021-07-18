using System;
using System.Security.Cryptography;

namespace MiniCRMCore
{
	public static class Hasher
	{
		/// <summary>
		/// Размер хэша.
		/// </summary>
		private const int HashLength = 20;

		/// <summary>
		/// Число итераций.
		/// </summary>
		private const int Iterations = 10000;

		/// <summary>
		/// Вычисляет хэш строки с паролем и отдает его в виде Base64 строки.
		/// Используется "подсаливание" байтами из <paramref name="salt" />
		/// </summary>
		/// <param name="password">пароль</param>
		/// <param name="salt">криптографическая "соль"</param>
		/// <returns>кодированный в Base64 хэш пароля</returns>
		public static string ComputeHash(string password, Guid salt)
		{
			return ComputeHash(password, salt, HashAlgorithmName.SHA256);
		}

		public static string ComputeHash(string password, Guid salt, HashAlgorithmName hashAlgorithmName)
		{
			if (string.IsNullOrWhiteSpace(password)) return string.Empty;

			using var deriveBytes = new Rfc2898DeriveBytes(password, salt.ToByteArray(), Iterations, hashAlgorithmName);
			var key = deriveBytes.GetBytes(HashLength);
			return Convert.ToBase64String(key);
		}
	}
}