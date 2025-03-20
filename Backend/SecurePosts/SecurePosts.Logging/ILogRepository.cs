using System;
using System.Collections.Generic;
using System.Text;

namespace SecurePosts.Logging
{
    public interface ILogRepository
    {
        void LogError(string errorMessage);
    }
}
