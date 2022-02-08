using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using MiniCRMCore.Areas.Auth.Models;
using MiniCRMCore.Utilities.Exceptions;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace MiniCRMCore.Areas.Auth
{
    public class AuthService
    {
        private readonly ApplicationContext _context;
        private readonly IConfiguration _configuration;
        private readonly IMapper _mapper;

        public AuthService(
            ApplicationContext context,
            IConfiguration configuration,
            IMapper mapper)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public async Task<User.AuthResponseDto> RegisterAsync(User.RegisterDto registerDto)
        {
            var user = new User
            {
                Login = registerDto.Login,
                Role = Role.Manager,
                Name = registerDto.Name,
                Email = registerDto.Email
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

        public async Task<User.Dto> GetManagerAsync(int id)
        {
            var manager = await _context.Users.FirstOrDefaultAsync(x => x.Id == id);
            if (manager == null)
                throw new ApiException($"Не найден менеджер с ID {id}");

            var dto = _mapper.Map<User.Dto>(manager);
            return dto;
        }

        public async Task<List<User.Dto>> GetManagersAsync()
        {
            var managers = await _context.Users.Where(x => x.Role == Role.Manager).OrderBy(x => x.Id).ToListAsync();
            var dto = _mapper.Map<List<User.Dto>>(managers);
            return dto;
        }

        public async Task<User.Dto> UpdateManagerAsync(User.Dto updateDto)
        {
            var manager = await _context.Users.FirstOrDefaultAsync(x => x.Id == updateDto.Id);
            if (manager == null)
                throw new ApiException($"Не найден менеджер с ID {updateDto.Id}");

            manager.Name = updateDto.Name;
            manager.Login = updateDto.Login;
            manager.Email = updateDto.Email;

            await _context.SaveChangesAsync();

            var dto = _mapper.Map<User.Dto>(manager);
            return dto;
        }

        public async Task ChangeManagerPasswordAsync(User.NewPasswordDto dto, int currentUserId)
        {
            var manager = await _context.Users.FirstOrDefaultAsync(x => x.Id == dto.ManagerId);
            if (manager == null)
                throw new ApiException($"Не найден менеджер с ID {dto.ManagerId}");

            var currentUser = await _context.Users.FirstOrDefaultAsync(x => x.Id == currentUserId);
            if (currentUser.Role != Role.Administrator)
                throw new ApiException("Операция запрещена", 403);

            if (dto.Password != dto.PasswordConfirm)
                throw new ApiException("Пароли не совпадают", 400);

            if (manager.CheckPassword(dto.Password))
                throw new ApiException("Пароль совпадает со старым", 400);

            manager.SetPassword(dto.Password, Guid.NewGuid());

            await _context.SaveChangesAsync();
        }

        public async Task DeleteManagerAsync(int id)
        {
            var manager = await _context.Users.FirstOrDefaultAsync(x => x.Id == id);
            if (manager == null)
                throw new ApiException($"Не найден менеджер с ID {id}");

            _context.Users.Remove(manager);
            await _context.SaveChangesAsync();
        }
    }
}