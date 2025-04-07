using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using SecureService.Context;
using SecureService.DAL.Repositories;
using SecureService.Entity.Shared.Database;
using SecureService.Entity.Shared.Status;
using SecureService.Logging;

namespace SecureService.DAL.Services
{
    public class DalLogoutService : IDalLogoutRepository
    {
        private readonly ILogRepository _logger;
        private readonly SecureServiceDbContext _context;

        public DalLogoutService(ILogRepository logger,
            SecureServiceDbContext context)
        {
            this._logger = logger;
            this._context = context;
        }

        public StatusResult<object> Logout(UserDetail loggedInUser)
        {
            StatusResult<object> status = new StatusResult<object>();
            try
            {
                if (loggedInUser != null)
                {

                    var usersSessions = _context.UserSession.Where(it => it.UserId == loggedInUser.UserId && it.IsActiveSessionFlag == true).ToList();
                    foreach (UserSession userSession in usersSessions)
                    {
                        userSession.IsActiveSessionFlag = false;
                        _context.UserSession.Update(userSession);
                        _context.SaveChanges();
                    }

                    var usersRefreshTokens = _context.UserRefreshToken.Where(it => it.UserId == loggedInUser.UserId && it.IsActive == true).ToList();
                    foreach (UserRefreshToken userRefreshToken in usersRefreshTokens)
                    {
                        userRefreshToken.IsActive = false;
                        _context.UserRefreshToken.Update(userRefreshToken);
                        _context.SaveChanges();
                    }

                    status.Status = "OK";
                    status.Message = "Logged Out Successfully.";
                    status.Result = null;
                }
                else
                {
                    status.Status = "OK";
                    status.Message = null;
                    status.Result = null;
                }
                return status;
            }
            catch (Exception ex)
            {
                _logger.LogError("Error in Logout: " + ex.Message + " , Stacktrace : " + ex.StackTrace);
                status.Status = "FAILED";
                status.Message = "Logged Out Failed.";
                status.Result = null;
                return status;
            }
        }
    }
}
