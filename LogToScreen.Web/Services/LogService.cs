using LogToScreen.Web.Hubs;
using Microsoft.AspNetCore.SignalR;
using System;
using System.IO;
using System.Text;

namespace LogToScreen.Web.Services
{
    public class LogService : ILogService
    {
        private readonly ILogFilesWatcher _logFilesWatcher;
        private readonly ILogHubWrapper _logwrapper;

        public LogService(ILogFilesWatcher logFilesWatcher, ILogHubWrapper logwrapper)
        {
            _logFilesWatcher = logFilesWatcher ?? throw new ArgumentNullException(nameof(logFilesWatcher));
            _logwrapper = logwrapper ?? throw new ArgumentNullException(nameof(logwrapper));

            _logFilesWatcher.OnChangedHandler += OnChanged;
        }

        private async void OnChanged(object sender, FileSystemEventArgs e)
        {            
            if (e.ChangeType != WatcherChangeTypes.Changed)
            {
                return;
            }

            try
            {
                string content;
                using (var fileStream = new FileStream(e.FullPath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
                {
                    using (var streamReader = new StreamReader(fileStream, Encoding.Default))
                    {
                        content = streamReader.ReadToEnd();
                    }
                }

                await _logwrapper.SendToClientsAsync(content);
            }
            catch
            {
                //to be thinked
            }
            
        }
    }
}
