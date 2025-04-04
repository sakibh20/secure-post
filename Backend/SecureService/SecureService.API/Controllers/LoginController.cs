using Microsoft.AspNetCore.Mvc;
using SecureService.BLL.Repositories;
using SecureService.Entity.Shared.Internal;

namespace SecureService.API.Controllers
{
    [ApiController]
    [Route("api/[action]")]
    public class LoginController : Controller
    {
        private readonly ILoginRepository _ILoginRepository;

        public LoginController(ILoginRepository ILoginRepository)
        {
            this._ILoginRepository = ILoginRepository;
        }

        [HttpPost]
        public IActionResult Login(LoginViewModel loginModel)
        {
            return Ok(_ILoginRepository.Login(loginModel));
        }
    }
}
