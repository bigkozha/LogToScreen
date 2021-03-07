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
        private readonly IHubContext<LogHub> _logContext;

        public LogService(ILogFilesWatcher logFilesWatcher, IHubContext<LogHub> logContext)
        {
            _logFilesWatcher = logFilesWatcher ?? throw new ArgumentNullException(nameof(logFilesWatcher));
            _logContext = logContext ?? throw new ArgumentNullException(nameof(logContext));

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


                await _logContext.Clients.All.SendAsync("ReceiveMessage", content);
            }
            catch
            {
                //to be thinked
            }
            
        }
    }
}
