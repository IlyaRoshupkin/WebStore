using Microsoft.AspNetCore.Identity;

namespace WebStore.Domain.Entityes.Identity
{
    public class User : IdentityUser
    {
        public string Description { get; set; }
        public const string Administrator = "Administrator";
        public const string DefaultAdminPassword = "AdPass";
    }
    public class Role : IdentityRole
    {
        public const string Administrator = "Administrators";
        public const string User = "Users";
        public string Description { get; set; }
    }
}
