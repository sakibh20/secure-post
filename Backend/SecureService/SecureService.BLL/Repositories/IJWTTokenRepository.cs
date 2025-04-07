using SecureService.Entity.Shared.Database;
using SecureService.Entity.Shared.Internal;
using System.Security.Claims;

namespace SecureService.BLL.Repositories
{
    public interface IJWTTokenRepository
    {
        public UserDetail ValidateAccessToken(ClaimsIdentity identity);
        public UserMatch ValidateMatchToken(ClaimsIdentity identity);
        public TokenResponseViewModel GenerateTokenByRefreshToken(ClaimsIdentity identity);
    }
}
