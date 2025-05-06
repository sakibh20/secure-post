using System;
using System.IO;
using System.Reflection;
using log4net;
using log4net.Config;
using Microsoft.Extensions.Configuration;

namespace SecureService.Logging
{
    public class LogService : ILogRepository
    {
        private readonly IConfiguration _config;

        public LogService(IConfiguration config)
        {
            this._config = config;
        }

        public void LogError(string errorMessage)
        {
            try
            {
                var logRepository = LogManager.GetRepository(Assembly.GetEntryAssembly());
                XmlConfigurator.Configure(logRepository, new FileInfo("log4net.config"));

                //string messageFormat = $"************************************************************************************************************* {Environment.NewLine}{Environment.NewLine}{DateTime.UtcNow} <======> {errorMessage} {Environment.NewLine}{Environment.NewLine}*************************************************************************************************************{Environment.NewLine}";

                string messageFormat = $"{DateTime.UtcNow} <======> {errorMessage} {Environment.NewLine}{Environment.NewLine}*************************************************************************************************************{Environment.NewLine}{Environment.NewLine}";

                StreamWriter sw = null;
                string sLogFormat = DateTime.UtcNow.ToShortDateString().ToString() + " " + DateTime.UtcNow.ToLongTimeString().ToString() + " ==> ";
                string sYear = DateTime.UtcNow.Year.ToString();
                string sMonth = DateTime.UtcNow.Month.ToString();
                string sDay = DateTime.UtcNow.Day.ToString();
                string sErrorTime = sDay + "-" + sMonth + "-" + sYear;
                string userMobile = string.Empty;
                string product = string.Empty;
                string path = AppDomain.CurrentDomain.BaseDirectory + "/Error";
                if (!(Directory.Exists(path)))
                {
                    Directory.CreateDirectory(path);
                }

                string File = path + "/" + sErrorTime + ".txt";
                sw = new StreamWriter(File, true);


                sw.WriteLine(messageFormat);
                sw.Flush();
                if (sw != null)
                {
                    sw.Dispose();
                    sw.Close();
                }

                //errorLog.InfoFormat(messageFormat);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

    }
}
