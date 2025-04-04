using SecureService.Entity.Shared.Database;
using System.Security.Claims;

namespace SecureService.BLL.Repositories
{
    public interface IJWTTokenRepository
    {
        public UserDetail GetUserByHeaderToken(ClaimsIdentity identity);
    }
}
