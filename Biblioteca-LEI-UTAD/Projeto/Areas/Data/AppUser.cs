using Microsoft.AspNetCore.Identity;

namespace Trabalho.Areas.Data
{
    public class AppUser : IdentityUser
    {
        public string Nome { get; set; }
        public string Role { get; set; }
        public string? Address { get; set; }
        public string? PhoneNumber2 { get; set; }
        public string? PhoneNumber3 { get; set; }
    }
}
