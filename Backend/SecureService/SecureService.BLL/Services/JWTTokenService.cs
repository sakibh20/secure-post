
using System.Security.Claims;
using SecureService.BLL.Repositories;
using SecureService.DAL.Repositories;
using SecureService.Entity.Shared.Database;
using SecureService.Entity.Shared.Internal;

namespace SecureService.BLL.Services
{
    public class JWTTokenService : IJWTTokenRepository
    {
        private readonly IDalJWTTokenRepository _IDalJWTTokenRepository;

        public JWTTokenService(IDalJWTTokenRepository IDalJWTTokenRepository)
        {
            this._IDalJWTTokenRepository = IDalJWTTokenRepository;
        }

        public UserDetail ValidateAccessToken(ClaimsIdentity identity)
        {
            return _IDalJWTTokenRepository.ValidateAccessToken(identity); 
        }
        public UserMatch ValidateMatchToken(ClaimsIdentity identity)
        {
            return _IDalJWTTokenRepository.ValidateMatchToken(identity);
        }
        public TokenResponseViewModel GenerateTokenByRefreshToken(ClaimsIdentity identity)
        {
            return _IDalJWTTokenRepository.GenerateTokenByRefreshToken(identity);
        }

    }
}
