
using System.Security.Claims;
using SecurePosts.BLL.Repositories;
using SecurePosts.DAL.Repositories;
using SecurePosts.Entity.Shared.Database;

namespace SecurePosts.BLL.Services
{
    public class JWTTokenService : IJWTTokenRepository
    {
        private readonly IDalJWTTokenRepository _IDalJWTTokenRepository;

        public JWTTokenService(IDalJWTTokenRepository IDalJWTTokenRepository)
        {
            this._IDalJWTTokenRepository = IDalJWTTokenRepository;
        }

        public UserDetail GetUserByHeaderToken(ClaimsIdentity identity)
        {
            return _IDalJWTTokenRepository.GetUserByHeaderToken(identity); 
        }
    }
}
