using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json.Linq;
using SecureService.Context;
using SecureService.DAL.Repositories;
using SecureService.Entity.Shared.Database;
using SecureService.Entity.Shared.Internal;
using SecureService.Entity.Shared.Status;
using SecureService.Logging;

namespace SecureService.DAL.Services
{
    public class DalMatchService : IDalMatchRepository
    {
        private readonly IConfiguration _config;
        private readonly ILogRepository _logger;
        private readonly IDalClsRepository _cls;
        private readonly IDalJWTTokenRepository _jwt;
        private readonly SecureServiceDbContext _context;

        public DalMatchService(ILogRepository logger,
            IConfiguration config,
            IDalClsRepository cls,
            IDalJWTTokenRepository jwt,
            SecureServiceDbContext context)
        {
            this._logger = logger;
            this._config = config;
            this._cls = cls;
            this._jwt = jwt;
            this._context = context;
        }

        public StatusResult<object> CreateMatchID(PlayerValidationViewModel playerInfo)
        {
            StatusResult<object> status = new StatusResult<object>();
            try
            {
                UserDetail Validplayer1 = new UserDetail();
                bool IsValidplayer1 = false;
                UserDetail Validplayer2 = new UserDetail();
                bool IsValidplayer2 = false;

                var handler = new JwtSecurityTokenHandler();

                #region Player 1
                var player1AccessToken = handler.ReadToken(playerInfo.player1AccessToken);
                var player1AccessTokenClaims = player1AccessToken as JwtSecurityToken;

                var player1UserId = _cls.Decrypt(player1AccessTokenClaims.Claims.First(claim => claim.Type == "UserId").Value);
                var player1SessionID = _cls.Decrypt(player1AccessTokenClaims.Claims.First(claim => claim.Type == "SessionID").Value);
                if (!string.IsNullOrWhiteSpace(player1UserId) && playerInfo.player1 == player1UserId)
                {
                    Validplayer1 = _context.UserDetail.Where(it => it.UserId.Equals(player1UserId)).FirstOrDefault();

                    if (Validplayer1 != null)
                    {
                        var checkActiveSession = _context.UserSession.Where(it => it.UserId == Validplayer1.UserId && it.SessionID == player1SessionID && it.IsActiveSessionFlag == true).FirstOrDefault();
                        if (checkActiveSession != null)
                        {
                            IsValidplayer1 = true;
                        }
                        else
                            throw new Exception("Token Deactivated.");
                    }
                    else
                        throw new Exception("Invalid Token.");
                }
                else
                    throw new Exception("Invalid User.");
                #endregion

                #region Player 2
                var player2AccessToken = handler.ReadToken(playerInfo.player2AccessToken);
                var player2AccessTokenClaims = player2AccessToken as JwtSecurityToken;

                var player2UserId = _cls.Decrypt(player2AccessTokenClaims.Claims.First(claim => claim.Type == "UserId").Value);
                var player2SessionID = _cls.Decrypt(player2AccessTokenClaims.Claims.First(claim => claim.Type == "SessionID").Value);
                if (!string.IsNullOrWhiteSpace(player2UserId) && playerInfo.player2 == player2UserId)
                {
                    Validplayer2 = _context.UserDetail.Where(it => it.UserId.Equals(player2UserId)).FirstOrDefault();

                    if (Validplayer2 != null)
                    {
                        var checkActiveSession = _context.UserSession.Where(it => it.UserId == Validplayer2.UserId && it.SessionID == player2SessionID && it.IsActiveSessionFlag == true).FirstOrDefault();
                        if (checkActiveSession != null)
                        {
                            IsValidplayer2 = true;
                        }
                        else
                            throw new Exception("Token Deactivated.");
                    }
                    else
                        throw new Exception("Invalid Token.");
                }
                else
                    throw new Exception("Invalid User.");
                #endregion

                #region Match ID Genaration
                if(IsValidplayer1 && IsValidplayer2)
                { 
                    UserMatch userNewMatch = new UserMatch();
                    userNewMatch.MatchId = Guid.NewGuid().ToString();
                    userNewMatch.Player1 = Validplayer1.UserId;
                    userNewMatch.Player2 = Validplayer2.UserId;

                    _context.UserMatch.Add(userNewMatch);
                    if(_context.SaveChanges() > 0)
                    {
                        MatchTokenViewModel matchToken = _jwt.GenerateMatchToken(userNewMatch);
                        if(matchToken != null)
                        {
                            status.Status = "OK";
                            status.Message = "Match Id Created";
                            status.Result = matchToken;
                        }
                    }
                }
                #endregion
            }
            catch (Exception ex)
            {
                _logger.LogError("Error in CreateMatchID : " + ex.Message + " , Stacktrace: " + ex.StackTrace);
                status.Status = "FAILED";
                status.Message = ex.Message;
                status.Result = null;
            }
            return status;
        }
        public StatusResult<object> UpdateMatchResult(MatchresultViewModel matchInfo, UserMatch match)
        {
            StatusResult<object> status = new StatusResult<object>();
            try
            {
                if (matchInfo.MatchId != match.MatchId)
                    throw new Exception("Invalid Match ID.");

                match.Player1Moves= matchInfo.Player1Moves;
                match.Player2Moves= matchInfo.Player2Moves;
                match.Winner= matchInfo.Winner;
                match.EndTime = DateTime.Now;

                _context.UserMatch.Update(match);
                if (_context.SaveChanges() > 0)
                {
                    status.Status = "OK";
                    status.Message = "Match Info Updated.";
                    status.Result = null;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError("Error in UpdateMatchResult : " + ex.Message + " , Stacktrace: " + ex.StackTrace);
                status.Status = "FAILED";
                status.Message = ex.Message;
                status.Result = null;
            }
            return status;
        }
        public StatusResult<object> FetchLeaderBoard()
        {
            StatusResult<object> status = new StatusResult<object>();
            try
            {
                var leaderboard = _context.UserMatch
                    .Where(match => !string.IsNullOrEmpty(match.Winner))
                    .GroupBy(match => match.Winner)
                    .Select(it => new
                    {
                        Player = it.Key,
                        Wins = it.Count()
                    })
                    .OrderByDescending(x => x.Wins)
                    .Take(10)
                    .ToList();

                status.Status = "SUCCESS";
                status.Message = "Leaderboard fetched successfully.";
                status.Result = leaderboard;
            }
            catch (Exception ex)
            {
                _logger.LogError("Error in FetchLeaderBoard: " + ex.Message + " , Stacktrace: " + ex.StackTrace);
                status.Status = "FAILED";
                status.Message = ex.Message;
                status.Result = null;
            }
            return status;
        }
        public StatusResult<object> FetchMatchHistory(UserDetail user)
        {
            StatusResult<object> status = new StatusResult<object>();
            try
            {
                List<UserMatch> matchHistory = _context.UserMatch.Where(it => (it.Player1 == user.UserId || it.Player2 == user.UserId) && it.EndTime != null).ToList();

                status.Status = "SUCCESS";
                status.Message = "Matches History fetched successfully.";
                status.Result = matchHistory;
            }
            catch (Exception ex)
            {
                _logger.LogError("Error in FetchMatchHistory: " + ex.Message + " , Stacktrace: " + ex.StackTrace);
                status.Status = "FAILED";
                status.Message = ex.Message;
                status.Result = null;
            }
            return status;
        }

    }
}
