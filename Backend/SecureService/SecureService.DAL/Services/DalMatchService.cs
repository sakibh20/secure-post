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

        public StatusResult<object> InitializeMatchRequest(string playerID, UserDetail user)
        {
            StatusResult<object> status = new StatusResult<object>();
            try
            {
                var player2 = _context.UserDetail.Where(it => it.UserId == playerID).FirstOrDefault();
                if (player2 != null)
                {
                    UserMatch userNewMatch = new UserMatch();
                    userNewMatch.MatchId = Guid.NewGuid().ToString();
                    userNewMatch.Player1 = user.UserId;
                    userNewMatch.Player2 = player2.UserId;

                    _context.UserMatch.Add(userNewMatch);
                    _context.SaveChanges();

                    //Server A Rest API call


                    MatchRequestViewModel match = new MatchRequestViewModel();
                    match.MatchId = userNewMatch.MatchId;
                    match.Player1 = userNewMatch.Player1;
                    match.Player2 = userNewMatch.Player2;
                    match.MatchStatus = userNewMatch.Status;

                    status.Status = "OK";
                    status.Message = user.UserId + " has requested " + player2.UserId + " for a Match.";
                    status.Result = null;

                }
                else
                    throw new Exception("Invalid Player ID.");
            }
            catch (Exception ex)
            {
                _logger.LogError("Error in InitializeMatchRequest : " + ex.Message + " , Stacktrace: " + ex.StackTrace);
                status.Status = "FAILED";
                status.Message = ex.Message;
                status.Result = null;
            }
            return status;
        }
        public StatusResult<object> ResponseMatchRequest(MatchRequestViewModel match, UserDetail user)
        {
            StatusResult<object> status = new StatusResult<object>();
            try
            {
                #region Match ID Genaration
                if (match.Player2 == user.UserId)
                {
                    UserMatch userExistingMatch = _context.UserMatch.Where(it => it.MatchId == match.MatchId && it.Player1 == match.Player1 && it.Player2 == match.Player2).FirstOrDefault();

                    if (userExistingMatch == null)
                        throw new Exception("Invalid match Information.");
                    else if (userExistingMatch.Status?.ToUpper() != "PENDING")
                        throw new Exception("Match is already " + userExistingMatch.Status.ToLower());

                    if (match.MatchStatus?.ToUpper() == "ACCEPTED")
                    {
                        userExistingMatch.Status = "ACCEPTED";
                        userExistingMatch.StartTime = DateTime.Now;
                        _context.UserMatch.Update(userExistingMatch);
                        _context.SaveChanges();


                        string matchToken = _jwt.GenerateMatchToken(userExistingMatch);

                        MatchAcceptedViewModel matchAccepted = new MatchAcceptedViewModel();
                        matchAccepted.MatchId = userExistingMatch.MatchId;
                        matchAccepted.Player1 = userExistingMatch.Player1;
                        matchAccepted.Player2 = userExistingMatch.Player2;
                        matchAccepted.FirstTurn = matchAccepted.Player1;
                        matchAccepted.MatchToken = matchToken;
                        _logger.LogError("Token-" + matchToken);
                        //Server A Rest API call


                        status.Status = "OK";
                        status.Message = userExistingMatch.Player2 + " has accepted " + userExistingMatch.Player1 + "'s Match Request.";
                        status.Result = null;
                    }
                    else if (match.MatchStatus?.ToUpper() == "DECLINED")
                    {
                        userExistingMatch.Status = "DECLINED";
                        _context.UserMatch.Update(userExistingMatch);
                        _context.SaveChanges();

                        status.Status = "OK";
                        status.Message = userExistingMatch.Player2 + " has declined " + userExistingMatch.Player1 + "'s Match Request.";
                        status.Result = null;
                    }
                    else
                        throw new Exception("Invalid Match Status.");
                }
                else
                    throw new Exception("Invalid Player 2.");
                #endregion
            }
            catch (Exception ex)
            {
                _logger.LogError("Error in ResponseMatchRequest : " + ex.Message + " , Stacktrace: " + ex.StackTrace);
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
                else if (match.Status?.ToUpper() != "ACCEPTED")
                    throw new Exception("Match status is " + match.Status.ToLower());

                match.Player1Moves = matchInfo.Player1Moves;
                match.Player2Moves= matchInfo.Player2Moves;
                match.Winner= matchInfo.Winner;
                match.EndTime = DateTime.Now;
                match.Status = "FINNISHED";

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
        public StatusResult<object> FetchLeaderBoard(UserDetail user)
        {
            StatusResult<object> status = new StatusResult<object>();
            try
            {
                LeaderBoardDataViewModel leaderBoardData = new LeaderBoardDataViewModel();

                var allUsersdata = _context.UserMatch
                                    .Where(match => !string.IsNullOrEmpty(match.Winner))
                                    .GroupBy(match => match.Winner)
                                    .Select(it => new
                                    {
                                        Player = it.Key,
                                        Wins = it.Count()
                                    })
                                    .OrderByDescending(x => x.Wins)
                                    .ToList() 
                                    .Select((it, index) => new LeaderBoardUserData
                                    {
                                        Position = (index + 1).ToString(),
                                        Player = it.Player,
                                        Wins = it.Wins
                                    })
                                    .ToList();

                leaderBoardData.TopUsers = allUsersdata.Take(10).ToList();
                leaderBoardData.User = allUsersdata.Where(it=> it.Player == user.UserId).FirstOrDefault();

                if(leaderBoardData.User == null)
                {
                    leaderBoardData.User = new LeaderBoardUserData();
                    leaderBoardData.User.Position = "-";
                    leaderBoardData.User.Player = user.UserId;
                    leaderBoardData.User.Wins = 0;
                }

                status.Status = "OK";
                status.Message = "Leaderboard fetched successfully.";
                status.Result = leaderBoardData;
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
