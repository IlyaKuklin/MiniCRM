using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.DependencyInjection;
using MiniCRMCore.Utilities.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniCRMCore.Areas.Auth
{
    public class JwtTokenProvider
    {
        private readonly ApplicationContext _context;
        public JwtTokenProvider( ApplicationContext contex)
        {
            _context = contex;
        }

        public static Task OnTokenValidated(TokenValidatedContext context)
        {
            var userId = context.Principal.GetUserId();

            var ctx = context.HttpContext.RequestServices.GetService<ApplicationContext>();

            var user = ctx.Users.FirstOrDefault(x => x.Id == userId);
            if (user == null)
                throw new ApiException("Доступ запрещён", 403);

            return Task.CompletedTask;
        }
    }
}
