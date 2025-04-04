using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;
using SecureService.Entity.Shared.Database;

namespace SecureService.DAL.Repositories
{
    public interface IDalJWTTokenRepository
    {
        public UserDetail GetUserByHeaderToken(ClaimsIdentity identity);
        public string GenerateJWTToken(UserSession usersNewSession);
    }
}
