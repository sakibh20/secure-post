using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using SecurePosts.Context;
using SecurePosts.DAL.Repositories;
using SecurePosts.Entity.Shared.Database;
using SecurePosts.Logging;

namespace SecurePosts.DAL.Services
{
    public class DalJWTTokenService: IDalJWTTokenRepository
    {
        private readonly IConfiguration _config;
        private readonly ILogRepository _logger;
        private readonly IDalClsRepository _cls;
        private readonly SecurePostsDbContext _context;

        public DalJWTTokenService(ILogRepository logger, 
            IConfiguration config,
            IDalClsRepository cls,
            SecurePostsDbContext context)
        {
            this._logger = logger;
            this._config = config;
            this._cls = cls;
            this._context = context;
        }

        public UserDetail GetUserByHeaderToken(ClaimsIdentity identity)
        {
            List<UserSession> usersSessions = new List<UserSession>();
            List<UserDetail> registeredUsers = new List<UserDetail>();
            DateTime nowDate = DateTime.Now;
            DateTime ExpiryDate = DateTime.Now;
            UserDetail user = new UserDetail();
            UserDetail Validuser = new UserDetail();
            string SessionID = string.Empty;

            if (identity != null)
            {
                IEnumerable<Claim> claims = identity.Claims;
                user.UserId = _cls.Decrypt(identity.FindFirst("UserId")?.Value);
                ExpiryDate = _cls.ConvertToDateTime(identity.FindFirst("ExpiryDate")?.Value);
                SessionID = _cls.Decrypt(identity.FindFirst("SessionID")?.Value);

            }

            if (string.IsNullOrEmpty(user.UserId) && string.IsNullOrEmpty(SessionID))
                throw new Exception("Vaild Token Required.");

            if (nowDate > ExpiryDate)
            {
                //return null; //Time expired
                throw new Exception("Token Expired.");
            }

            if (user != null && !string.IsNullOrEmpty(user.UserId))
            {

                if (!string.IsNullOrWhiteSpace(user.UserId))
                {
                    Validuser = _context.UserDetail.Where(it => it.UserId.Equals(user.UserId)).FirstOrDefault();

                    if (Validuser != null)
                    {
                        var checkActiveSession = _context.UserSession.Where(it => it.UserId == Validuser.UserId && it.SessionID == SessionID && it.IsActiveSessionFlag == true).FirstOrDefault();
                        if (checkActiveSession != null)
                        {
                            return Validuser;
                        }
                        else
                            throw new Exception("Token Deactivated.");
                    }
                    else
                        throw new Exception("Invalid Token.");
                }
                else
                    throw new Exception("Invalid Token.");
            }
            throw new Exception("Invalid Token.");
        }
        public string GenerateJWTToken(UserSession usersNewSession)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new[] {
                new Claim("UserId", _cls.Encrypt(usersNewSession.UserId)),
                new Claim("SessionID", _cls.Encrypt(usersNewSession.SessionID)),
                new Claim("StartDate", usersNewSession.SessionStartTime.ToString()),
                new Claim("ExpiryDate", usersNewSession.SessionEndTime.ToString())
            };
            var token = new JwtSecurityToken(_config["Jwt:Issuer"],
              _config["Jwt:Issuer"],
              claims,
              expires: usersNewSession.SessionEndTime,
              signingCredentials: credentials);
            var JWTToken = new JwtSecurityTokenHandler().WriteToken(token);

            return JWTToken;
        }
    }
}
