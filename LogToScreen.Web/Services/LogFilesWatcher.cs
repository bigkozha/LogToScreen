using LogToScreen.Web.Hubs;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace LogToScreen.Web.Services
{
    public class LogFilesWatcher : ILogFilesWatcher, IDisposable
    {
        public FileSystemEventHandler OnChangedHandler { get; set; }
        private readonly string _pathWithLogs;
        private bool disposedValue = false;
        private FileSystemWatcher _watcher;

        public LogFilesWatcher(string pathWithLogs)
        {
            _pathWithLogs = Path.GetFullPath(pathWithLogs) ?? throw new ArgumentNullException(nameof(pathWithLogs));
            InitWatcher();
        }

        public void Dispose()
        {
            Dispose(true);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    _watcher.Error -= OnError;
                    _watcher.Changed -= OnChangedHandler;
                    _watcher.Dispose();
                }

                disposedValue = true;
            }
        }

        private void InitWatcher()
        {
            _watcher = new FileSystemWatcher(_pathWithLogs)
            {
                NotifyFilter = NotifyFilters.LastAccess
                             | NotifyFilters.LastWrite
                             | NotifyFilters.Size
            };

            _watcher.Changed += OnChanged;
            _watcher.Error += OnError;

            _watcher.Filter = "*.txt";
            _watcher.IncludeSubdirectories = false;
            _watcher.EnableRaisingEvents = true;
        }

        private void OnChanged(object sender, FileSystemEventArgs e)
        {
            OnChangedHandler.Invoke(sender, e);
        }

        private void OnError(object sender, ErrorEventArgs e)
        {
            PrintException(e.GetException());
        }

        private void PrintException(Exception ex)
        {
            if (ex != null)
            {
                Console.WriteLine($"Message: {ex.Message}");
                Console.WriteLine("Stacktrace:");
                Console.WriteLine(ex.StackTrace);
                Console.WriteLine();
                PrintException(ex.InnerException);
            }
        }
    }
}