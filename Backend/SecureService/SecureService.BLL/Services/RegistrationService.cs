using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SecureService.BLL.Repositories;
using SecureService.DAL.Repositories;
using SecureService.Entity.Shared.Internal;
using SecureService.Entity.Shared.Status;

namespace SecureService.BLL.Services
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
