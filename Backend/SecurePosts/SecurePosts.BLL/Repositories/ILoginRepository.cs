using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SecurePosts.Entity.Shared.Internal;
using SecurePosts.Entity.Shared.Status;

namespace SecurePosts.BLL.Repositories
{
    public interface ILoginRepository
    {
        public StatusResult<object> Login(LoginViewModel loginModel);
    }
}
