using Microsoft.AspNetCore.Identity;

namespace InventoryWebApp.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string FullName { get; set; }
    }
}
