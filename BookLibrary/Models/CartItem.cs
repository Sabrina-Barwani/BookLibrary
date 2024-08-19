using System.ComponentModel.DataAnnotations;

namespace BookLibrary.Models
{
    public class CartItem
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int CartId { get; set; }

        [Required]
        public int BookId { get; set; }

        [Required]
        public int Quantity { get; set; }

        // Navigation property for cart
        public Cart Cart { get; set; }

        // Navigation property for book
        public Book Book { get; set; }
    }
}
