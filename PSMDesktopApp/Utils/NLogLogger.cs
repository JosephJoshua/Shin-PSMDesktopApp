using Caliburn.Micro;
using PSMDesktopApp.Library.Api;
using System;
using System.Windows;

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
            string message;

            if (exception is ApiException)
            {
                var apiException = exception as ApiException;
                bool hasDetails = !string.IsNullOrEmpty(apiException.Details);

                Write(
                    NLog.LogLevel.Error, "{0}{1}{2}" + Environment.NewLine + "{3}",
                    apiException.Message,
                    hasDetails ? ": " : "",
                    hasDetails ? apiException.Details : "",
                    exception.StackTrace
                );

                message = $"Terjadi error. { apiException.Message }{ (hasDetails ? ": " : "") }{ apiException.Details }" +
                    Environment.NewLine + exception.StackTrace;
            }
            else
            {
                Write(NLog.LogLevel.Error, exception.ToString()); 
                message = $"Terjadi error: { exception.ToString() }";
            }

            try
            {
                MessageBox.Show(message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (Exception ex)
            {
                Write(NLog.LogLevel.Error, ex.ToString());
            }
        }

        private void Write(NLog.LogLevel level, string format, params object[] args)
        {
            NLog.LogEventInfo eventInfo = new NLog.LogEventInfo(level, _logger.Name, null, format, args);
            _logger.Log(typeof(NLogLogger), eventInfo);
        }
    }
}
