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
