using System;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using SecurePosts.BLL.Repositories;
using SecurePosts.Entity.Shared.Internal;
using SecurePosts.Entity.Shared.Status;
using SecurePosts.Logging;

namespace SecurePosts.API.Controllers
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
        public IActionResult Register(RegistrationViewModel registrationModel)
        {
            return Ok(_IRegistrationRepository.Register(registrationModel));
        }
    }
}
