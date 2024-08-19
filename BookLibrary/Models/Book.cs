using BookLibrary.Models;
using System.ComponentModel.DataAnnotations;
using System.Security.Cryptography.X509Certificates;

namespace BookLibrary.Models
{
    public class Book
    {
        public int Id { get; set; }
        [Required]
        public string Title { get; set; }
        public string? Description { get; set; }
        public string? Author { get; set; }
        public string? Imageurl { get; set; }
        public DateTime PublishDate { get; set; }

       public int CategoryId { get; set; }
        public Category Category { get; set; }

        public IEnumerable<User>? Books { get; set; } = new HashSet<User>();

        // Navigation property for cart items
        public ICollection<CartItem> CartItems { get; set; }


    }
}
