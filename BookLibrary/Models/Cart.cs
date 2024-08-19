using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BookLibrary.Models
{
    public class Cart
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int UserId { get; set; }

        [DataType(DataType.Date)]
        public DateTime CreatedAt { get; set; }

        // Navigation property for cart items
        public ICollection<CartItem> CartItems { get; set; }

        // Navigation property for user
        public User User { get; set; }
    }
}

