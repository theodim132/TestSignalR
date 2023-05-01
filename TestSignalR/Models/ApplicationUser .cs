using Microsoft.AspNetCore.Identity;

namespace TestSignalR.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string? ImagePath { get; set; }
    }
}
