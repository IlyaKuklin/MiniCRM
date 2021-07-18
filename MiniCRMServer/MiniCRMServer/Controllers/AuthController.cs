using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MiniCRMCore.Areas.Auth;
using MiniCRMCore.Areas.Auth.Models;
using System;
using System.Threading.Tasks;

namespace MiniCRMServer.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
	public class AuthController : ControllerBase
	{
		private readonly AuthService _authService;

		public AuthController(AuthService authService)
		{
			_authService = authService ?? throw new ArgumentNullException(nameof(authService));
		}

		[HttpPost("register")]
		[ProducesResponseType(typeof(User.AuthResponseDto), 200)]
		[Authorize(Roles = "Administrator")]
		public async Task<IActionResult> Register([FromBody] User.RegisterDto registerDto)
		{
			var result = await _authService.RegisterAsync(registerDto);
			return this.Ok(result);
		}

		[HttpPost("login")]
		[ProducesResponseType(typeof(User.AuthResponseDto), 200)]
		[AllowAnonymous]
		public async Task<IActionResult> Login([FromBody] User.AuthDto authDto)
		{
			var result = await _authService.LoginAsync(authDto);
			return this.Ok(result);
		}
	}
}
