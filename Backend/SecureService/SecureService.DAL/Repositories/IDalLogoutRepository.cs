using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SecureService.Entity.Shared.Database;
using SecureService.Entity.Shared.Status;

namespace SecureService.DAL.Repositories
{
    public interface IDalLogoutRepository
    {
        public StatusResult<object> Logout(UserDetail loggedInUser);
    }
}
