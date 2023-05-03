using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using System.Diagnostics;
using TestSignalR.Hubs;
using TestSignalR.Models;
using TestSignalR.Services;

namespace TestSignalR.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ServiceBusListener _serviceBusListener;
        private readonly ServiceBusHub _hub;

        public HomeController(ILogger<HomeController> logger, ServiceBusListener serviceBusListener, ServiceBusHub hub)
        {
            _logger = logger;
            _serviceBusListener = serviceBusListener;
            _serviceBusListener.RegisterOnMessageHandlerAndReceiveMessages(HandleMessage);
            _hub = hub;
        }
        public IActionResult Index()
        {
            return View();
        }
        private async Task HandleMessage(string message)
        {
            // Send the message to the connected clients via SignalR
            await _hub.Clients.All.SendAsync("ReceiveMessage", message);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}