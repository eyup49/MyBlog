using Microsoft.AspNetCore.Identity;

namespace MyPageBlog.Mvc.Identity
{
    public class User:IdentityUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }
}