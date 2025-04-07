using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SecureService.BLL.Repositories;
using SecureService.DAL.Repositories;
using SecureService.Entity.Shared.Database;
using SecureService.Entity.Shared.Status;

namespace SecureService.BLL.Services
{
    public class LogoutService : ILogoutRepository
    {
        private readonly IDalLogoutRepository _IDalLogoutRepository;

        public LogoutService(IDalLogoutRepository IDalLogoutRepository)
        {
            this._IDalLogoutRepository = IDalLogoutRepository;
        }
        public StatusResult<object> Logout(UserDetail loggedInUser)
        {
            return _IDalLogoutRepository.Logout(loggedInUser);
        }
    }
}
