using LogToScreen.Web.Hubs;
using LogToScreen.Web.Services;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LogToScreen.Web.Controllers
{
    public class LogController : Controller
    {
        private readonly ILogService _logService;
        
        public LogController(ILogService logService)
        {
            _logService = logService ?? throw new ArgumentNullException(nameof(LogFilesWatcher));
        }

        public IActionResult Index()
        {
            return View();
        }
    }
}
