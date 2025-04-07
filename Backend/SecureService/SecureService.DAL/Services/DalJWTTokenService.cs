using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using SecureService.Context;
using SecureService.DAL.Repositories;
using SecureService.Entity.Shared.Database;
using SecureService.Entity.Shared.Internal;
using SecureService.Logging;

namespace SecureService.DAL.Services
{
    public class DalJWTTokenService: IDalJWTTokenRepository
    {
        private readonly IConfiguration _config;
        private readonly ILogRepository _logger;
        private readonly IDalClsRepository _cls;
        private readonly SecureServiceDbContext _context;

        public DalJWTTokenService(ILogRepository logger, 
            IConfiguration config,
            IDalClsRepository cls,
            SecureServiceDbContext context)
        {
            this._logger = logger;
            this._config = config;
            this._cls = cls;
            this._context = context;
        }

        public UserDetail ValidateAccessToken(ClaimsIdentity identity)
        {
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

        public UserMatch ValidateMatchToken(ClaimsIdentity identity)
        {
            DateTime nowDate = DateTime.Now;
            DateTime ExpiryDate = DateTime.Now;
            string MatchID = string.Empty;
            string UserId = string.Empty;

            if (identity != null)
            {
                IEnumerable<Claim> claims = identity.Claims;
                UserId = _cls.Decrypt(identity.FindFirst("UserId")?.Value);
                ExpiryDate = _cls.ConvertToDateTime(identity.FindFirst("ExpiryDate")?.Value);
                MatchID = _cls.Decrypt(identity.FindFirst("SessionID")?.Value);

            }

            if (string.IsNullOrEmpty(UserId) && string.IsNullOrEmpty(MatchID))
                throw new Exception("Vaild Token Required.");

            if (nowDate > ExpiryDate)
            {
                //return null; //Time expired
                throw new Exception("Token Expired.");
            }

            if (!string.IsNullOrEmpty(UserId))
            {
                string player1 = UserId.Split('#')[0];
                string player2 = UserId.Split('#')[1];
                var checkActiveMatch = _context.UserMatch.Where(it => it.Player1 == player1 && it.Player2 == player2 && it.MatchId == MatchID && it.EndTime == null).FirstOrDefault();
                if (checkActiveMatch != null)
                {
                    return checkActiveMatch;
                }
                else
                    throw new Exception("Token Deactivated.");
            }
            throw new Exception("Invalid Token.");
        }

        public TokenResponseViewModel GenerateTokenByRefreshToken(ClaimsIdentity identity)
        {
            DateTime nowDate = DateTime.Now;
            DateTime ExpiryDate = DateTime.Now;
            UserDetail user = new UserDetail();
            UserDetail Validuser = new UserDetail();
            string TokenID = string.Empty;

            if (identity != null)
            {
                IEnumerable<Claim> claims = identity.Claims;
                user.UserId = _cls.Decrypt(identity.FindFirst("UserId")?.Value);
                ExpiryDate = _cls.ConvertToDateTime(identity.FindFirst("ExpiryDate")?.Value);
                TokenID = _cls.Decrypt(identity.FindFirst("TokenID")?.Value);

            }

            if (string.IsNullOrEmpty(user.UserId) && string.IsNullOrEmpty(TokenID))
                throw new Exception("Vaild Refresh Token Required.");

            if (nowDate > ExpiryDate)
            {
                //return null; //Time expired
                throw new Exception("Refresh Token Expired.");
            }

            if (user != null && !string.IsNullOrEmpty(user.UserId))
            {

                if (!string.IsNullOrWhiteSpace(user.UserId))
                {
                    Validuser = _context.UserDetail.Where(it => it.UserId.Equals(user.UserId)).FirstOrDefault();

                    if (Validuser != null)
                    {
                        var checkActiveRefreshToken = _context.UserRefreshToken.Where(it => it.UserId == Validuser.UserId && it.TokenId == TokenID && it.IsActive == true).FirstOrDefault();
                        if (checkActiveRefreshToken != null)
                        {
                            var usersSessions = _context.UserSession.Where(it => it.UserId == Validuser.UserId && it.IsActiveSessionFlag == true).ToList();
                            foreach (UserSession userSession in usersSessions)
                            {
                                userSession.IsActiveSessionFlag = false;
                                _context.UserSession.Update(userSession);
                                _context.SaveChanges();
                            }

                            UserSession usersNewSession = new UserSession();
                            usersNewSession.UserId = Validuser.UserId;
                            usersNewSession.SessionID = Guid.NewGuid().ToString();
                            usersNewSession.IsActiveSessionFlag = true;
                            usersNewSession.SessionStartTime = DateTime.Now;
                            usersNewSession.SessionEndTime = DateTime.Now.AddMinutes(15);
                            _context.UserSession.Add(usersNewSession);
                            if (_context.SaveChanges() > 0)
                            {
                                return GenerateToken(usersNewSession, checkActiveRefreshToken);
                            }
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

        public TokenResponseViewModel GenerateToken(UserSession usersNewSession, UserRefreshToken userNewRefreshToken)
        {
            TokenResponseViewModel token = new TokenResponseViewModel();
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var accessclaims = new[] {
                new Claim("UserId", _cls.Encrypt(usersNewSession.UserId)),
                new Claim("SessionID", _cls.Encrypt(usersNewSession.SessionID)),
                new Claim("StartDate", usersNewSession.SessionStartTime.ToString()),
                new Claim("ExpiryDate", usersNewSession.SessionEndTime.ToString())
            };
            var accessToken = new JwtSecurityToken(_config["Jwt:Issuer"],
              _config["Jwt:Issuer"],
              accessclaims,
              expires: usersNewSession.SessionEndTime,
              signingCredentials: credentials);
            var JWTAccessToken = new JwtSecurityTokenHandler().WriteToken(accessToken);

            token.AccessToken = JWTAccessToken;
            token.AccessTokenExpiry = Convert.ToDateTime(usersNewSession.SessionEndTime);

            var refreshClaims = new[] {
                new Claim("UserId", _cls.Encrypt(userNewRefreshToken.UserId)),
                new Claim("TokenID", _cls.Encrypt(userNewRefreshToken.TokenId)),
                new Claim("StartDate", userNewRefreshToken.CreatedAt.ToString()),
                new Claim("ExpiryDate", userNewRefreshToken.ExpiryDate.ToString())
            };
            var refreshToken = new JwtSecurityToken(_config["Jwt:Issuer"],
              _config["Jwt:Issuer"],
              refreshClaims,
              expires: userNewRefreshToken.ExpiryDate,
              signingCredentials: credentials);
            var JWTRefreshToken = new JwtSecurityTokenHandler().WriteToken(refreshToken);

            token.RefreshToken = JWTRefreshToken;
            token.RefreshTokenExpiry = Convert.ToDateTime(userNewRefreshToken.ExpiryDate);

            return token;
        }

        public MatchTokenViewModel GenerateMatchToken(UserMatch userNewMatch)
        {
            MatchTokenViewModel matchToken = new MatchTokenViewModel();
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var accessclaims = new[] {
                new Claim("UserId", _cls.Encrypt(userNewMatch.Player1 + "#" + userNewMatch.Player2)),
                new Claim("SessionID", _cls.Encrypt(userNewMatch.MatchId)),
                new Claim("StartDate", userNewMatch.StartTime.ToString()),
                new Claim("ExpiryDate", userNewMatch.StartTime.AddMinutes(30).ToString())
            };
            var accessToken = new JwtSecurityToken(_config["Jwt:Issuer"],
              _config["Jwt:Issuer"],
              accessclaims,
              expires: userNewMatch.StartTime.AddMinutes(30),
              signingCredentials: credentials);
            var JWTmatchToken = new JwtSecurityTokenHandler().WriteToken(accessToken);

            matchToken.matchID = userNewMatch.MatchId;
            matchToken.matchToken = JWTmatchToken;
            return matchToken;
        }

    }
}
