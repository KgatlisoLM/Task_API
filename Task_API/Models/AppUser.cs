using Microsoft.AspNetCore.Identity;

namespace Task_API.Models
{
    public class AppUser : IdentityUser
    {
        public string FullName { get; set; }
    }
}
