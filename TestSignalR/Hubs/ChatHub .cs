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

        public ChatHub(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task SendMessage(string user, string message)
        {
            // Save the message to the database
            var newMessage = new Message { UserName = user, Content = message };
            _context.Messages.Add(newMessage);
            await _context.SaveChangesAsync();

            // Broadcast the message to all clients
            await Clients.All.SendAsync("ReceiveMessage", user, message);
        }
    }
}
