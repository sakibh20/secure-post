using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SecureService.Entity.Shared.Internal;
using SecureService.Entity.Shared.Status;

namespace SecureService.BLL.Repositories
{
    public interface ILoginRepository
    {
        public StatusResult<object> Login(LoginViewModel loginModel);
    }
}
