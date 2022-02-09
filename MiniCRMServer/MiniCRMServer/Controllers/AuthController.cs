using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MiniCRMCore.Areas.Auth;
using MiniCRMCore.Areas.Auth.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;

namespace MiniCRMServer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Administrator")]
    public class AuthController : ControllerBase
    {
        private readonly AuthService _authService;

        public AuthController(AuthService authService)
        {
            _authService = authService ?? throw new ArgumentNullException(nameof(authService));
        }

        public int CurrentUserId => this.User.GetUserId();

        [HttpPost("register")]
        [ProducesResponseType(typeof(User.AuthResponseDto), 200)]
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

        [HttpGet("manager")]
        [ProducesResponseType(typeof(User.Dto), 200)]
        public async Task<IActionResult> GetManager([FromQuery] int id)
        {
            var result = await _authService.GetManagerAsync(id);
            return this.Ok(result);
        }

        [HttpGet("managers")]
        [ProducesResponseType(typeof(List<User.Dto>), 200)]
        public async Task<IActionResult> GetManagers()
        {
            var result = await _authService.GetManagersAsync();
            return this.Ok(result);
        }

        [HttpPatch("manager/update")]
        [ProducesResponseType(typeof(User.Dto), 200)]
        public async Task<IActionResult> UpdateManager([FromBody] User.Dto updateDto)
        {
            var result = await _authService.UpdateManagerAsync(updateDto);
            return this.Ok(result);
        }

        [HttpPatch("manager/changePassword")]
        [ProducesResponseType(200)]
        public async Task<IActionResult> ChangeManagerPassword([FromBody][Required] User.NewPasswordDto passwordDto)
        {
            await _authService.ChangeManagerPasswordAsync(passwordDto, this.CurrentUserId);
            return this.Ok(200);
        }

        [HttpDelete("manager/delete")]
        [ProducesResponseType(204)]
        public async Task<IActionResult> DeleteManager([FromQuery] int id)
        {
            await _authService.DeleteManagerAsync(id);
            return this.Ok(204);
        }
    }
}