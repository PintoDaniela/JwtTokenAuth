using Microsoft.AspNetCore.Identity;

namespace JwtTokenTest2.Models
{
    public class User
    {

        public int Id { get; set; }
        public string UserName { get; set; }
        public string PasswordHash { get; set; }
        public byte[] PasswordSalt { get; set; }
    }
}
