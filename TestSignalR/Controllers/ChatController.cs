using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TestSignalR.Data;
using TestSignalR.Models;

namespace TestSignalR.Controllers
{
    [Authorize]
    public class ChatController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public ChatController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public IActionResult Index()
        {
            var onlineUsers = _userManager.Users.ToList();

            return View(onlineUsers);
        }
        public IActionResult Test()
        {
            var onlineUsers = _userManager.Users.ToList();

            return View(onlineUsers);
        }
        [HttpGet]
        public async Task<IActionResult> GetMessages()
        {
            var messages = await _context.Messages.ToListAsync();
            return Json(messages);
        }
        [HttpGet]
        public JsonResult GetChatHistory(string currentUser, string otherUser)
        {
            var chatHistory = _context.Messages
        .Where(m => (m.SenderId == currentUser && m.ReceiverId == otherUser) || (m.SenderId == otherUser && m.ReceiverId == currentUser))
        .OrderBy(m => m.Timestamp) // Assuming there is a Timestamp property in the Message model
        .ToList();
            return Json(chatHistory);
        }
    }
}
