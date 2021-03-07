using System.IO;

namespace LogToScreen.Web.Services
{
    public interface ILogFilesWatcher
    {
        FileSystemEventHandler OnChangedHandler { get; set; }
    }
}