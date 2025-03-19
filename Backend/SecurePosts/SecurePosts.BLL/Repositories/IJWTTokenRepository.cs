using SecurePosts.Entity.Shared.Database;
using System.Security.Claims;

namespace SecurePosts.BLL.Repositories
{
    public interface IJWTTokenRepository
    {
        public UserDetail GetUserByHeaderToken(ClaimsIdentity identity);
    }
}
