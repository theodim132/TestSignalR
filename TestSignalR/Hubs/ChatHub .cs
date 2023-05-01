using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using TestSignalR.Data;
using TestSignalR.Models;

namespace TestSignalR.Hubs
{
    public class ChatHub : Hub
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;


        public ChatHub(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }
        //public async Task SendMessage(string targetUserId, string content)
        //{
        //    var sender = await _userManager.GetUserAsync(Context.User);
        //    var groupName = GetGroupName(sender.Id, targetUserId);

        //    // Save the message to the database
        //    var message = new Message
        //    {
        //        SenderId = sender.Id,
        //        ReceiverId = targetUserId,
        //        Content = content,
        //        Timestamp = DateTime.UtcNow
        //    };
        //    _context.Messages.Add(message);
        //    await _context.SaveChangesAsync();

        //    // Send the message to the appropriate group
        //    await Clients.Group(groupName).SendAsync("ReceiveMessage", targetUserId, sender.UserName, content, message.Timestamp);

        //}
        public async Task SendMessage(string targetUserId, string message)
        {
            string senderUserId = Context.UserIdentifier;
            var Newmessage = new Message
            {
                SenderId = senderUserId,
                ReceiverId = targetUserId,
                Content = message,
                Timestamp = DateTime.Now
            };
            _context.Messages.Add(Newmessage);
            await _context.SaveChangesAsync();
            // Broadcast the message to the sender and the target user
            await Clients.Users(new List<string> { senderUserId, targetUserId }).SendAsync("ReceiveMessage", senderUserId, targetUserId, message, DateTime.Now.ToString("hh:mm tt"));
        }

        public async Task JoinChat(string targetUserId)
        {
            var sender = await _userManager.GetUserAsync(Context.User);
            var groupName = GetGroupName(sender.Id, targetUserId);

            await Groups.AddToGroupAsync(Context.ConnectionId, groupName);
        }

        public async Task LeaveChat(string targetUserId)
        {
            var sender = await _userManager.GetUserAsync(Context.User);
            var groupName = GetGroupName(sender.Id, targetUserId);

            await Groups.RemoveFromGroupAsync(Context.ConnectionId, groupName);
        }

        private string GetGroupName(string user1Id, string user2Id)
        {
            var compareResult = string.CompareOrdinal(user1Id, user2Id);
            return compareResult < 0 ? $"{user1Id}-{user2Id}" : $"{user2Id}-{user1Id}";
        }
        public async Task<IEnumerable<Message>> GetChatHistory(string targetUserId)
        {
            var sender = await _userManager.GetUserAsync(Context.User);
            var messages = await _context.Messages
                .Where(m => (m.SenderId == sender.Id && m.ReceiverId == targetUserId) || (m.SenderId == targetUserId && m.ReceiverId == sender.Id))
                .OrderBy(m => m.Timestamp)
                .ToListAsync();

            return messages;
        }

        //public async Task SendMessage(string user, string message)
        //{
        //    // Save the message to the database
        //    var newMessage = new Message { UserName = user, Content = message };
        //    _context.Messages.Add(newMessage);
        //    await _context.SaveChangesAsync();

        //    // Broadcast the message to all clients
        //    await Clients.All.SendAsync("ReceiveMessage", user, message);
        //}
    }
}
