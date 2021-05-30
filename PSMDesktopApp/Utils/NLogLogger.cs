using Caliburn.Micro;
using PSMDesktopApp.Library.Api;
using System;

namespace PSMDesktopApp.Utils
{
    public class NLogLogger : ILog
    {
        private readonly NLog.Logger _logger;

        public NLogLogger(Type type)
        {
            _logger = NLog.LogManager.GetLogger(type.FullName);
        }

        public void Info(string format, params object[] args)
        {
            Write(NLog.LogLevel.Info, format, args);
        }

        public void Warn(string format, params object[] args)
        {
            Write(NLog.LogLevel.Warn, format, args);
        }

        public void Error(Exception exception)
        {
            if (exception is ApiException)
            {
                var apiException = exception as ApiException;
                bool hasDetails = !string.IsNullOrEmpty(apiException.Details);

                Write(
                    NLog.LogLevel.Error, "{0}{1}{2}",
                    apiException.Message,
                    hasDetails ? ": " : "",
                    hasDetails ? apiException.Details : ""
                );
            }
            else
            {
                Write(NLog.LogLevel.Error, exception.ToString());
            }
        }

        private void Write(NLog.LogLevel level, string format, params object[] args)
        {
            NLog.LogEventInfo eventInfo = new NLog.LogEventInfo(level, _logger.Name, null, format, args);
            _logger.Log(typeof(NLogLogger), eventInfo);
        }
    }
}
