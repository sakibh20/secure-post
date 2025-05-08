using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using SecureService.Context;
using SecureService.DAL.Repositories;
using SecureService.Entity.Shared.Database;
using SecureService.Entity.Shared.Internal;
using SecureService.Entity.Shared.Status;
using SecureService.Logging;

namespace SecureService.DAL.Services
{
    public class DalRegistrationService : IDalRegistrationRepository
    {
        private readonly IConfiguration _config;
        private readonly ILogRepository _logger;
        private readonly IDalClsRepository _cls;
        private readonly SecureServiceDbContext _context;

        public DalRegistrationService(ILogRepository logger,
            IConfiguration config,
            IDalClsRepository cls,
            SecureServiceDbContext context)
        {
            this._logger = logger;
            this._config = config;
            this._cls = cls;
            this._context = context;
        }

        public StatusResult<object> Register(string encryptData)
        {
            StatusResult<object> status = new StatusResult<object>();
            try
            {
                RegistrationViewModel registrationModel = JsonConvert.DeserializeObject<RegistrationViewModel>(_cls.Decrypt(encryptData));
                #region Validation
                if (!Regex.IsMatch(registrationModel.UserId, @"^[A-Za-z0-9]{1,10}$"))
                    throw new Exception("Invalid User ID. User ID should be within 10 digits and contain only alphanumeric characters.");

                if (registrationModel.Password != registrationModel.ConfirmPassword)
                    throw new Exception("Invalid Password. Confirm password doesn't match with provided password.");
                if (registrationModel.Password.Length < 8)
                    throw new Exception("Password must be at least 8 characters long.");
                if (!Regex.IsMatch(registrationModel.Password, @"[A-Z]"))
                    throw new Exception("Password must contain at least one uppercase letter.");
                if (!Regex.IsMatch(registrationModel.Password, @"[a-z]"))
                    throw new Exception("Password must contain at least one lowercase letter.");
                if (!Regex.IsMatch(registrationModel.Password, @"\d")) 
                    throw new Exception("Password must contain at least one digit.");
                if (!Regex.IsMatch(registrationModel.Password, @"[\W_]+")) 
                    throw new Exception("Password must contain at least one special character (e.g., @, #, $, etc.).");

                if (!Regex.IsMatch(registrationModel.Email, @"^[^@\s]+@[^@\s]+\.[^@\s]+$"))
                    throw new Exception("Invalid Email.");

                var checkForExistingUserByUserID = _context.UserDetail.Where(it=>it.UserId.Equals(registrationModel.UserId)).FirstOrDefault();

                if (checkForExistingUserByUserID != null)
                    throw new Exception(registrationModel.UserId + " is already existed.");

                var checkForExistingUserByEmail = _context.UserDetail.Where(it => it.Email.Equals(registrationModel.Email)).FirstOrDefault();

                if (checkForExistingUserByEmail != null)
                    throw new Exception(registrationModel.Email + " is already existed.");
                #endregion

                #region New User
                UserDetail newUser = new UserDetail();
                newUser.UserId = registrationModel.UserId;
                newUser.UserName = registrationModel.UserName;
                newUser.Email = registrationModel.Email;
                newUser.Password = _cls.EncryptSha256Hash(registrationModel.Password);
                newUser.CreatedAt = DateTime.UtcNow;
                newUser.Role = "USER";

                _context.UserDetail.Add(newUser);

                if (_context.SaveChanges() > 0)
                {
                    status.Status = "OK";
                    status.Message = "Successfully Registered.";
                    status.Result = null;
                }
                #endregion
            }
            catch (Exception ex) 
            {
                _logger.LogError("Error in Register : " + ex.Message + " , Stacktrace: " + ex.StackTrace);
                status.Status = "FAILED";
                status.Message = ex.Message;
                status.Result = null;
            }
            return status;
        }
    }
}
