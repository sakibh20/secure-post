using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;
using SecureService.Entity.Shared.Database;
using SecureService.Entity.Shared.Internal;

namespace SecureService.DAL.Repositories
{
    public interface IDalJWTTokenRepository
    {
        public UserDetail ValidateAccessToken(ClaimsIdentity identity);
        public UserMatch ValidateMatchToken(ClaimsIdentity identity);
        public TokenResponseViewModel GenerateTokenByRefreshToken(ClaimsIdentity identity);
        public TokenResponseViewModel GenerateToken(UserSession usersNewSession, UserRefreshToken userNewRefreshToken);
        public string GenerateMatchToken(UserMatch userNewMatch);
    }
}
