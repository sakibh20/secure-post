using System;
using System.Collections.Generic;
using System.Text;

namespace SecureService.Logging
{
    public interface ILogRepository
    {
        void LogError(string errorMessage);
    }
}
