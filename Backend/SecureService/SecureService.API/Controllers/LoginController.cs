using System.Security.Claims;
using System;
using Microsoft.AspNetCore.Mvc;
using SecureService.BLL.Repositories;
using SecureService.Entity.Shared.Internal;
using SecureService.Entity.Shared.Status;

namespace SecureService.API.Controllers
{
    [ApiController]
    [Route("api/[action]")]
    public class LoginController : Controller
    {
        private readonly ILoginRepository _ILoginRepository;
        private readonly IJWTTokenRepository _IJWTTokenRepository;

        public LoginController(ILoginRepository ILoginRepository, IJWTTokenRepository iJWTTokenRepository)
        {
            this._ILoginRepository = ILoginRepository;
            _IJWTTokenRepository = iJWTTokenRepository;
        }

        [HttpPost]
        public IActionResult Login(LoginViewModel loginModel)
        {
            return Ok(_ILoginRepository.Login(loginModel));
        }

        [HttpGet]
        public IActionResult GenerateAccessTokenByRefreshToken()
        {
            StatusResult<object> status = new StatusResult<object>();
            TokenResponseViewModel token;
            try 
            {
                var identity = HttpContext.User.Identity as ClaimsIdentity;
                token = _IJWTTokenRepository.GenerateTokenByRefreshToken(identity);
                if (token != null)
                {
                    status.Status = "OK";
                    status.Message = "Generated Access Token";
                    status.Result = token;
                    return Ok(status);
                }
                else
                    throw new Exception("Access Token Genaration Failed.");
            }
            catch (Exception ex)
            {
                status.Status = "UNAUTH";
                status.Message = ex.Message;
                status.Result = null;
                return Ok(status);
            } 
        }
    }
}
