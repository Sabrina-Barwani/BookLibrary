using BookLibrary.Data;
using BookLibrary.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BookLibrary.Controllers
{
    public class CategoryController : Controller
    {
        private readonly ApplicationDbContext _dbContext;

        public CategoryController(ApplicationDbContext dbContext)
        {
                        _dbContext = dbContext;
        }
        public IActionResult Library()
        {

            List<Category> categories = _dbContext.Categories.ToList();
            return View(categories);
        }
        public IActionResult Book()
        {
            List<Book> books = _dbContext.Books.ToList();
            return View(books);
        }
      
        public IActionResult UserHome()
        {
            var categories = _dbContext.Categories.ToList();
            return View(categories);
        }
        public IActionResult BooksByCategory(int id)
        {
            var books = _dbContext.Books.Where(b => b.CategoryId == id).ToList();
            var category = _dbContext.Categories.FirstOrDefault(c => c.Id == id);

            if (category == null)
            {
                return NotFound();
            }

            ViewBag.CategoryName = category.Name;

            // Get cart items from session
            var cartItems = HttpContext.Session.Get<List<int>>("CartItems") ?? new List<int>();
            ViewBag.CartItems = cartItems;

            return View(books);
        }
        public IActionResult AddToCart(int bookId)
        {
            // Check if the user is logged in
            if (HttpContext.Session.GetString("UserLoggedIn") == null)
            {
                return RedirectToAction("Login", "Account");
            }

            // Retrieve the logged-in user
            var username = HttpContext.Session.GetString("UserLoggedIn");
            var user = _dbContext.Users.FirstOrDefault(u => u.Username == username);
            if (user == null) return RedirectToAction("Login", "Account");

            // Retrieve or create the user's cart
            var cart = _dbContext.Carts.Include(c => c.CartItems)
                                       .FirstOrDefault(c => c.UserId == user.Id);

            if (cart == null)
            {
                cart = new Cart { UserId = user.Id, CreatedAt = DateTime.Now };
                _dbContext.Carts.Add(cart);
                _dbContext.SaveChanges(); // Save to get the cart ID
            }

            // Add or update the cart item
            var existingCartItem = _dbContext.CartItems
                                             .FirstOrDefault(ci => ci.CartId == cart.Id && ci.BookId == bookId);

            if (existingCartItem != null)
            {
                existingCartItem.Quantity++;
            }
            else
            {
                _dbContext.CartItems.Add(new CartItem { CartId = cart.Id, BookId = bookId, Quantity = 1 });
            }

            _dbContext.SaveChanges();

            return RedirectToAction("Cart");
        }




        public IActionResult Cart()
        {
            var username = HttpContext.Session.GetString("UserLoggedIn");
            if (username == null)
            {
                return RedirectToAction("Login", "Account");
            }

            var user = _dbContext.Users.FirstOrDefault(u => u.Username == username);
            if (user == null) return RedirectToAction("Login", "Account");

            // Retrieve the user's cart with related cart items and books
            var cart = _dbContext.Carts.Include(c => c.CartItems)
                                       .ThenInclude(ci => ci.Book)
                                       .FirstOrDefault(c => c.UserId == user.Id);

            if (cart == null || !cart.CartItems.Any())
            {
                return View(new List<Book>()); // No items in cart
            }

            var booksInCart = cart.CartItems.Select(ci => ci.Book).ToList();
            return View(booksInCart);
        }

        public IActionResult RemoveFromCart(int bookId)
        {
            // Check if the user is logged in
            if (HttpContext.Session.GetString("UserLoggedIn") == null)
            {
                return RedirectToAction("Login", "Account");
            }

            // Retrieve the logged-in user
            var username = HttpContext.Session.GetString("UserLoggedIn");
            var user = _dbContext.Users.FirstOrDefault(u => u.Username == username);
            if (user == null) return RedirectToAction("Login", "Account");

            // Retrieve the user's cart
            var cart = _dbContext.Carts.Include(c => c.CartItems)
                                       .FirstOrDefault(c => c.UserId == user.Id);
            if (cart == null)
            {
                return RedirectToAction("Cart"); // If no cart exists, redirect to cart view
            }

            // Find the cart item to remove
            var cartItem = _dbContext.CartItems
                                     .FirstOrDefault(ci => ci.CartId == cart.Id && ci.BookId == bookId);
            if (cartItem != null)
            {
                _dbContext.CartItems.Remove(cartItem); // Remove the item from the cart
                _dbContext.SaveChanges(); // Save changes to the database
            }

            return RedirectToAction("Cart"); // Redirect to cart view
        }



        // GET: /Category/ManageInfo
        public IActionResult ManageInfo()
        {
            var username = HttpContext.Session.GetString("UserLoggedIn");
            var user = _dbContext.Users.FirstOrDefault(u => u.Username == username);

            if (user == null)
            {
                return RedirectToAction("Login", "Account");
            }

            return View(user);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateUserInfo(User updatedUser)
        {
            var username = HttpContext.Session.GetString("UserLoggedIn");
            var user = _dbContext.Users.FirstOrDefault(u => u.Username == username);

            if (user == null)
            {
                return RedirectToAction("Login", "Account");
            }

            user.Name = updatedUser.Name;
            user.Phone = updatedUser.Phone;

            _dbContext.Users.Update(user);
            await _dbContext.SaveChangesAsync();

            return RedirectToAction("ManageInfo");
        }
    }
}







    

