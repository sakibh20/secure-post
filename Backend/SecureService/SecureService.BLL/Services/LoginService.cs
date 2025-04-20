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
    public class LoginService : ILoginRepository
    {

        private readonly IDalLoginRepository _IDalLoginRepository;

        public LoginService(IDalLoginRepository IDalLoginRepository)
        {
            this._IDalLoginRepository = IDalLoginRepository;
        }

        public StatusResult<object> Login(LoginViewModel loginModel)
        {
            return _IDalLoginRepository.Login(loginModel);
        }
    }
}
