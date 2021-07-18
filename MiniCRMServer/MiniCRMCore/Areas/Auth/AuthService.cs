using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using MiniCRMCore.Areas.Auth.Models;
using MiniCRMCore.Utilities.Exceptions;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace MiniCRMCore.Areas.Auth
{
	public class AuthService
	{
		private readonly ApplicationContext _context;
		private readonly IConfiguration _configuration;

		public AuthService(
			ApplicationContext context,
			IConfiguration configuration)
		{
			_context = context ?? throw new ArgumentNullException(nameof(context));
			_configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
		}

		public async Task<User.AuthResponseDto> RegisterAsync(User.RegisterDto registerDto)
		{
			var user = new User
			{
				Login = registerDto.Login,
				Role = Role.Manager,
				Name = registerDto.Name
			};

			user.SetPassword(registerDto.Password, Guid.NewGuid());
			await _context.Users.AddAsync(user);
			await _context.SaveChangesAsync();

			var dto = await this.GenerateJwtTokenAsync(user);
			return dto;
		}

		public async Task<User.AuthResponseDto> LoginAsync(User.AuthDto authDto)
		{
			var user = await _context.Users
				.FirstOrDefaultAsync(x => x.Login == authDto.Login);
			if (user == null)
				throw new ApiException($"Аккаунт с логином '{authDto.Login}' не найден.");

			if (!user.CheckPassword(authDto.Password))
				throw new ApiException("Ошибка авторизации. Проверьте правильность заполнения логина и пароля.");
			return await this.GenerateJwtTokenAsync(user);
		}

		/// <summary>
		/// Возвращает jwt-токен для отправки на клиент.
		/// </summary>
		/// <returns></returns>
		private async Task<User.AuthResponseDto> GenerateJwtTokenAsync(User user)
		{
			if (user == null) throw new ArgumentNullException(nameof(user));

			var result = new User.AuthResponseDto();

			await Task.Run(() =>
			{
				var claims = new List<Claim>
				{
					new Claim(ClaimsProvider.Consts.USER_ID, user.Id.ToString()),
					new Claim(ClaimsProvider.Consts.LOGIN, user.Login),
					new Claim(ClaimsProvider.Consts.ROLE, user.Role.ToString())
				};
				claims.Add(new Claim(ClaimTypes.Role, Enum.GetName(typeof(Role), user.Role)));

				var expires = DateTime.Now.AddDays(Convert.ToDouble(_configuration["JwtExpireDays"]));
				var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JwtKey"]));
				var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
				var token = new JwtSecurityToken(
					issuer: _configuration["JwtIssuer"],
					audience: _configuration["JwtIssuer"],
					claims: claims,
					expires: expires,
					signingCredentials: creds);

				result = new User.AuthResponseDto
				{
					Token = new JwtSecurityTokenHandler().WriteToken(token),
					Login = user.Login,
					Id = user.Id,
					Role = user.Role,
					Name = user.Name
				};
			});

			return result;
		}
	}
}
