using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using System.Collections.Concurrent;
using TestSignalR.Data;
using TestSignalR.Models;

namespace TestSignalR.Hubs
{
    public class TestHub : Hub
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private static ConcurrentDictionary<string, string> _userConnectionMap = new ConcurrentDictionary<string, string>();

        public TestHub(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }
        public async Task SendMessage(string targetUserId, string message)
        {
            var currentUser = await _userManager.GetUserAsync(Context.User);
            var targetUserConnectionId = GetConnectionIdForUserId(targetUserId);
            if (!string.IsNullOrEmpty(targetUserConnectionId))
            {
                await Clients.Client(targetUserConnectionId).SendAsync("ReceiveMessage", currentUser.UserName, message);
            }
        }

        public async Task NotifyOnline()
        {
            var currentUser = await _userManager.GetUserAsync(Context.User);
            await Clients.Others.SendAsync("UserOnline", currentUser.Id, currentUser.UserName);
        }

        public async Task NotifyTyping(string targetUserId)
        {
            var currentUser = await _userManager.GetUserAsync(Context.User);
            var targetUserConnectionId = GetConnectionIdForUserId(targetUserId);
            if (!string.IsNullOrEmpty(targetUserConnectionId))
            {
                await Clients.Client(targetUserConnectionId).SendAsync("UserTyping", currentUser.UserName);
            }
        }

        public override async Task OnConnectedAsync()
        {
            var currentUser = await _userManager.GetUserAsync(Context.User);
            if (currentUser != null)
            {
                _userConnectionMap.AddOrUpdate(currentUser.Id, Context.ConnectionId, (key, oldValue) => Context.ConnectionId);
                await NotifyOnline();
            }
            await base.OnConnectedAsync();
        }
        public override async Task OnDisconnectedAsync(Exception exception)
        {
            var currentUser = await _userManager.GetUserAsync(Context.User);
            if (currentUser != null)
            {
                _userConnectionMap.TryRemove(currentUser.Id, out _);
            }
            await base.OnDisconnectedAsync(exception);
        }
        // You will need to create a dictionary or a method to map UserId to ConnectionId
        // and keep it updated whenever a user connects or disconnects.
        private string GetConnectionIdForUserId(string userId)
        {
            _userConnectionMap.TryGetValue(userId, out string connectionId);
            return connectionId;
        }
    }
}
