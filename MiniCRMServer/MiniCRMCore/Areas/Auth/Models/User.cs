using System;

namespace MiniCRMCore.Areas.Auth.Models
{
	public class User
	{
		public int Id { get; set; }

		public string Login { get; set; }

		public string PasswordHash { get; set; }

		public Guid Salt { get; set; }

		public bool IsDeleted { get; set; }

		public Role Role { get; set; }

		public string Name { get; set; }

		public string Email { get; set; }

		public void SetPassword(string password, Guid salt)
		{
			this.Salt = salt;
			this.PasswordHash = Hasher.ComputeHash(password, salt);
		}

		public bool CheckPassword(string password)
		{
			return string.IsNullOrEmpty(this.PasswordHash)
				? false
				: this.PasswordHash == Hasher.ComputeHash(password, this.Salt);
		}

		public class Dto
		{
			public int Id { get; set; }

			/// <summary>
			/// Имя пользователя.
			/// </summary>
			public string Name { get; set; }

			/// <summary>
			/// Адрес электронной почты.
			/// </summary>
			public string Login { get; set; }

			/// <summary>
			/// Email
			/// </summary>
			public string Email { get; set; }
		}

		/// <summary>
		/// Модель регистрации пользователя.
		/// </summary>
		public class RegisterDto
		{
			/// <summary>
			/// Имя пользователя.
			/// </summary>
			public string Name { get; set; }

			/// <summary>
			/// Адрес электронной почты.
			/// </summary>
			public string Login { get; set; }

			/// <summary>
			/// Пароль.
			/// </summary>
			public string Password { get; set; }

			/// <summary>
			/// Email
			/// </summary>
			public string Email { get; set; }
		}

		/// <summary>
		/// Модель аутентификации пользователя.
		/// </summary>
		public class AuthDto
		{
			/// <summary>
			/// Пароль.
			/// </summary>
			public string Password { get; set; }

			/// <summary>
			/// Login.
			/// </summary>
			public string Login { get; set; }
		}

		/// <summary>
		/// Модель ответа аутентификации пользователя.
		/// </summary>
		public class AuthResponseDto
		{
			public int Id { get; set; }

			/// <summary>
			/// JWT-токен доступа.
			/// </summary>
			public string Token { get; set; }

			/// <summary>
			/// Login пользователя.
			/// </summary>
			public string Login { get; set; }

			/// <summary>
			/// Роль пользователя.
			/// </summary>
			public Role Role { get; set; }

			public string Name { get; set; }
		}

		/// <summary>
		/// Модель восстановления пароля.
		/// </summary>
		public class PassworResetDto : AuthDto
		{
			/// <summary>
			/// Код из email.
			/// </summary>
			public string Code { get; set; }

			/// <summary>
			/// Подтверждения пароля.
			/// </summary>
			public string PasswordConfirm { get; set; }
		}

		/// <summary>
		/// Модель смены пароля.
		/// </summary>
		public class NewPasswordDto
		{
			/// <summary>
			/// Текущий пароль.
			/// </summary>
			public string CurrentPassword { get; set; }

			/// <summary>
			/// Новый пароль.
			/// </summary>
			public string Password { get; set; }

			/// <summary>
			/// Подтверждения нового пароля.
			/// </summary>
			public string PasswordConfirm { get; set; }
		}
	}

	public enum Role
	{
		Undefined = 0,
		NotSet = 1,

		Administrator = 2,
		Manager = 3
	}
}