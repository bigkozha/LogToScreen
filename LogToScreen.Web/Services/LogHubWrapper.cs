using LogToScreen.Web.Hubs;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LogToScreen.Web.Services
{
    public class LogHubWrapper : ILogHubWrapper
    {
        private readonly IHubContext<LogHub> _logContext;

        public LogHubWrapper(IHubContext<LogHub> logContext)
        {
            _logContext = logContext ?? throw new ArgumentNullException(nameof(logContext));
        }

        public async Task SendToClientsAsync(string logContent)
        {
            await _logContext.Clients.All.SendAsync("ReceiveMessage", logContent);
        }
    }
}
