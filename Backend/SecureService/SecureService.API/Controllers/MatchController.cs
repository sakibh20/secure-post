using System.Security.Claims;
using System;
using Microsoft.AspNetCore.Mvc;
using SecureService.BLL.Repositories;
using SecureService.Entity.Shared.Internal;
using SecureService.Entity.Shared.Status;
using SecureService.Entity.Shared.Database;

namespace SecureService.API.Controllers
{

    [ApiController]
    [Route("api/[action]")]
    public class MatchController : Controller
    {
        private readonly IMatchRepository _IMatchRepository;
        private readonly IJWTTokenRepository _IJWTTokenRepository;

        public MatchController(IMatchRepository IMatchRepository, IJWTTokenRepository iJWTTokenRepository)
        {
            this._IMatchRepository = IMatchRepository;
            _IJWTTokenRepository = iJWTTokenRepository;
        }
        
        [HttpGet]
        public IActionResult FetchUserInfo()
        {
            StatusResult<object> status = new StatusResult<object>();
            UserDetail loggedInUser;
            try
            {
                var identity = HttpContext.User.Identity as ClaimsIdentity;
                loggedInUser = _IJWTTokenRepository.ValidateAccessToken(identity);
                if (loggedInUser == null)
                {
                    status.Status = "FAILED";
                    status.Message = "Unauthorized Access.";
                    status.Result = null;
                    return Ok(status);
                }
                else
                {
                    var userInfo =  new
                    {
                        Name = loggedInUser.UserName,
                        Email = loggedInUser.Email
                    };
                    status.Status = "OK";
                    status.Message = "Fetching User Info Successful.";
                    status.Result = userInfo;
                    return Ok(status);
                }
            }
            catch (Exception ex)
            {
                status.Status = "FAILED";
                status.Message = ex.Message;
                status.Result = null;
                return Ok(status);
            }
        }

        [HttpGet]
        public IActionResult InitializeMatchRequest(string playerID)
        {
            StatusResult<object> status = new StatusResult<object>();
            UserDetail loggedInUser;
            try
            {
                var identity = HttpContext.User.Identity as ClaimsIdentity;
                loggedInUser = _IJWTTokenRepository.ValidateAccessToken(identity);
                if (loggedInUser == null)
                {
                    status.Status = "FAILED";
                    status.Message = "Unauthorized Access.";
                    status.Result = null;
                    return Ok(status);
                }
            }
            catch (Exception ex)
            {
                status.Status = "FAILED";
                status.Message = ex.Message;
                status.Result = null;
                return Ok(status);
            }
            return Ok(_IMatchRepository.InitializeMatchRequest(playerID, loggedInUser));
        }

        [HttpPost]
        public IActionResult ResponseMatchRequest(MatchRequestViewModel match)
        {
            StatusResult<object> status = new StatusResult<object>();
            UserDetail loggedInUser;
            try
            {
                var identity = HttpContext.User.Identity as ClaimsIdentity;
                loggedInUser = _IJWTTokenRepository.ValidateAccessToken(identity);
                if (loggedInUser == null)
                {
                    status.Status = "FAILED";
                    status.Message = "Unauthorized Access.";
                    status.Result = null;
                    return Ok(status);
                }
            }
            catch (Exception ex)
            {
                status.Status = "FAILED";
                status.Message = ex.Message;
                status.Result = null;
                return Ok(status);
            }
            return Ok(_IMatchRepository.ResponseMatchRequest(match, loggedInUser));
        }

        [HttpPost]
        public IActionResult UpdateMatchResult(MatchresultViewModel matchInfo)
        {
            StatusResult<object> status = new StatusResult<object>();
            UserMatch match;
            try
            {
                var identity = HttpContext.User.Identity as ClaimsIdentity;
                match = _IJWTTokenRepository.ValidateMatchToken(identity);
                if (match == null)
                {
                    status.Status = "FAILED";
                    status.Message = "Unauthorized Access.";
                    status.Result = null;
                    return Ok(status);
                }
            }
            catch (Exception ex)
            {
                status.Status = "FAILED";
                status.Message = ex.Message;
                status.Result = null;
                return Ok(status);
            }
            return Ok(_IMatchRepository.UpdateMatchResult(matchInfo, match));
        }

        [HttpGet]
        public IActionResult FetchLeaderBoard()
        {
            return Ok(_IMatchRepository.FetchLeaderBoard());
        }

        [HttpGet]
        public IActionResult FetchMatchHistory()
        {
            StatusResult<object> status = new StatusResult<object>();
            UserDetail loggedInUser;
            try
            {
                var identity = HttpContext.User.Identity as ClaimsIdentity;
                loggedInUser = _IJWTTokenRepository.ValidateAccessToken(identity);
                if (loggedInUser == null)
                {
                    status.Status = "FAILED";
                    status.Message = "Unauthorized Access.";
                    status.Result = null;
                    return Ok(status);
                }
            }
            catch (Exception ex)
            {
                status.Status = "FAILED";
                status.Message = ex.Message;
                status.Result = null;
                return Ok(status);
            }
            return Ok(_IMatchRepository.FetchMatchHistory(loggedInUser));
        }
                
    }
}
