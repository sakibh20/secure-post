using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SecurePosts.BLL.Repositories;
using SecurePosts.DAL.Repositories;
using SecurePosts.Entity.Shared.Internal;
using SecurePosts.Entity.Shared.Status;

namespace SecurePosts.BLL.Services
{
    public class RegistrationService : IRegistrationRepository
    {
        private readonly IDalRegistrationRepository _IDalRegistrationRepository;

        public RegistrationService(IDalRegistrationRepository IDalRegistrationRepository)
        {
            this._IDalRegistrationRepository = IDalRegistrationRepository;
        }
        public StatusResult<object> Register(RegistrationViewModel registrationModel)
        {
            return _IDalRegistrationRepository.Register(registrationModel);
        }
    }
}
