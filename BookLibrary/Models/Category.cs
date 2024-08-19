using System.ComponentModel.DataAnnotations;

namespace BookLibrary.Models
{
    public class Category
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }

        public IEnumerable<Book> Books { get; set; } = new HashSet<Book>();
    }
}
