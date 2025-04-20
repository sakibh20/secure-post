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
    public class LogoutController : Controller
    {
        private readonly ILogoutRepository _ILogoutRepository;
        private readonly IJWTTokenRepository _IJWTTokenRepository;

        public LogoutController(ILogoutRepository ILogoutRepository, IJWTTokenRepository iJWTTokenRepository)
        {
            this._ILogoutRepository = ILogoutRepository;
            this._IJWTTokenRepository = iJWTTokenRepository;
        }

        [HttpGet]
        public IActionResult Logout()
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
            return Ok(_ILogoutRepository.Logout(loggedInUser));
        }
    }
}
