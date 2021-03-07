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
    public class LogFilesWatcher : ILogFilesWatcher
    {
        public FileSystemEventHandler OnChangedHandler { get; set; }
        private readonly string _pathWithLogs;
        
        public LogFilesWatcher(string pathWithLogs)
        {
            _pathWithLogs = pathWithLogs ?? throw new ArgumentNullException(nameof(pathWithLogs));

            var watcher = new FileSystemWatcher(_pathWithLogs)
            {
                NotifyFilter = NotifyFilters.LastAccess
                                         | NotifyFilters.LastWrite
                                         | NotifyFilters.Size
            };

            watcher.Changed += (sender, e) => OnChangedHandler.Invoke(sender, e);
            watcher.Error += OnError;

            watcher.Filter = "*.txt";
            watcher.IncludeSubdirectories = false;
            watcher.EnableRaisingEvents = true;

            watcher.Disposed += (sender, e) =>
            {
                throw new Exception("pizdauskas");
            };
        }

        private static void OnError(object sender, ErrorEventArgs e)
        {
            PrintException(e.GetException());
        }

        private static void PrintException(Exception ex)
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