using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TestSignalR.Data;
using TestSignalR.Models;

namespace TestSignalR.Controllers
{
    public class ChatController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ChatController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public async Task<IActionResult> GetMessages()
        {
            var messages = await _context.Messages.ToListAsync();
            return Json(messages);
        }
  
    }
}
