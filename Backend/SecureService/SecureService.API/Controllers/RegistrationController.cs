using System;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using SecureService.BLL.Repositories;
using SecureService.Entity.Shared.Internal;
using SecureService.Entity.Shared.Status;
using SecureService.Logging;

namespace SecureService.API.Controllers
{
    [ApiController]
    [Route("api/[action]")]
    public class RegistrationController : ControllerBase
    {
        private readonly IRegistrationRepository _IRegistrationRepository;

        public RegistrationController(IRegistrationRepository IRegistrationRepository)
        {
            this._IRegistrationRepository = IRegistrationRepository;
        }

        [HttpPost]
        public IActionResult Register([FromForm]string encryptData)
        {
            return Ok(_IRegistrationRepository.Register(encryptData));
        }
    }
}
