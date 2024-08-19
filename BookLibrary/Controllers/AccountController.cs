using BookLibrary.Models;
using BookLibrary.Data;
using BookLibrary.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;

namespace BookLibrary.Controllers
{
    public class AccountController : Controller
    {
        private readonly ApplicationDbContext _dbContext;

        public AccountController(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        // GET: /Account/Register
        public IActionResult Register()
        {
            return View();
        }

        // POST: /Account/Register
        [HttpPost]
        [ValidateAntiForgeryToken]

        // parallel execution async task 
        public async Task<IActionResult> Register(User user)
        {
            if (ModelState.IsValid)
            {
                // Here you should hash the password before saving
                // Hash the password before saving
               // user.password = BCrypt.Net.BCrypt.HashPassword(user.Password);

                _dbContext.Users.Add(user);
                await _dbContext.SaveChangesAsync();
                return RedirectToAction("Login");
            }
            else
            {
                // Log or inspect ModelState errors here
                foreach (var error in ModelState.Values.SelectMany(v => v.Errors))
                {
                    // Do something with the errors
                    Console.WriteLine(error.ErrorMessage);

                }
                return View(user);
            }
            
        }

        // GET: /Account/Login
        public IActionResult Login()
        {
            return View();
        }

        // POST: /Account/Login
        [HttpPost]
        [ValidateAntiForgeryToken]

      //  [Authorize("admin,subadmin")]
        public IActionResult Login(string username, string password)
        {
            var user = _dbContext.Users
                .FirstOrDefault(u => u.Username.Equals(username) && u.password.Equals(password));

            if (user != null)
            {
                HttpContext.Session.SetString("UserLoggedIn", username);
                return RedirectToAction("UserHome", "Category"); // Redirect to the home page with categories
            }

            ModelState.AddModelError("", "Invalid login attempt.");
            return View();
        }



        // GET: /Account/Logout
        public IActionResult Logout()
        {
            HttpContext.Session.Remove("UserLoggedIn");
            return RedirectToAction("Index", "Home");
        }
    }
}
