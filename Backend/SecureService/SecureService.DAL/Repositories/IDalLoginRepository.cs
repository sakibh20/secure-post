﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SecureService.Entity.Shared.Internal;
using SecureService.Entity.Shared.Status;

namespace SecureService.DAL.Repositories
{
    public interface IDalLoginRepository
    {
        public StatusResult<object> Login(string encryptData);
    }
}
