using System.ComponentModel.DataAnnotations;

namespace InventoryWebApp.Models
{
    public class Supplier
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string Contact { get; set; }

        public string? Email { get; set; }
        public string? Address { get; set; }

        public string UserId { get; set; }  
    }
}