using System.Threading.Tasks;

namespace LogToScreen.Web.Services
{
    public interface ILogHubWrapper
    {
        Task SendToClientsAsync(string logContent);
    }
}