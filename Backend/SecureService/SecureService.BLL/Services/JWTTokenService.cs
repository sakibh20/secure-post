
using System.Security.Claims;
using SecureService.BLL.Repositories;
using SecureService.DAL.Repositories;
using SecureService.Entity.Shared.Database;

namespace SecureService.BLL.Services
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
