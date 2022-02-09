using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.DependencyInjection;
using MiniCRMCore.Utilities.Exceptions;
using System.Linq;
using System.Threading.Tasks;

namespace MiniCRMCore.Areas.Auth
{
    public class JwtTokenProvider
    {
        public static Task OnTokenValidated(TokenValidatedContext context)
        {
            var userId = context.Principal.GetUserId();

            // TODO: via MemoryCache
            var ctx = context.HttpContext.RequestServices.GetService<ApplicationContext>();
            var user = ctx.Users.FirstOrDefault(x => x.Id == userId);
            if (user == null || user.IsBlocked)
                throw new ApiException("Доступ запрещён", 403);

            return Task.CompletedTask;
        }
    }
}