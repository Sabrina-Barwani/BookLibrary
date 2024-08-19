using System.ComponentModel.DataAnnotations;

namespace BookLibrary.Models
{
    public class User
    {
        public int Id { get; set; } // Primary key column

        [Required]
        [MaxLength(50)] // Example length, adjust as needed
        public string Username { get; set; } // Unique but not primary key

        [Required]
        public string Name { get; set; }


        [Required]
        public string password { get; set; }


        public int? Phone { get; set; }

        // Navigation property for cart
        public ICollection<Cart>? Carts { get; set; }
    }
}
