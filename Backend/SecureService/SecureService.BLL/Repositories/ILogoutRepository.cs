using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SecureService.Entity.Shared.Database;
using SecureService.Entity.Shared.Status;

namespace SecureService.BLL.Repositories
{
    public interface ILogoutRepository
    {
        public StatusResult<object> Logout(UserDetail loggedInUser);
    }
}
