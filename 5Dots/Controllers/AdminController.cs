using _5Dots.Data;
using _5Dots.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace _5Dots.Controllers
{
    [Authorize(Roles = "ADMIN")]
    public class AdminController : Controller
    {
        private readonly ApplicationDbContext _context;
        public AdminController(ApplicationDbContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            var Id = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var user = _context.Users.Where(user => user.Id == Id).SingleOrDefault();
            ViewBag.Users = _context.Users.Where(user => user.Role == "User").ToList();
            ViewBag.Orders = _context.Orders.ToList();
            ViewBag.Categories = _context.Categories.ToList();
            ViewBag.Products = _context.Products.Include(product => product.Category).ToList();
            ViewBag.Payments = _context.Payments.Include(payment => payment.Order).ThenInclude(order => order.User).ToList();
            return View(user);
        }
        public IActionResult Users()
        {
            var Id = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var user = _context.Users.Where(user => user.Id == Id).SingleOrDefault();
            ViewBag.Users = _context.Users.Where(user => user.Role == "User").ToList();
            return View(user);
        }
        public IActionResult Categories()
        {
            var Id = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var user = _context.Users.Where(user => user.Id == Id).SingleOrDefault();
            ViewBag.Categories = _context.Categories.Include(category => category.Products).ToList();
            return View(user);
        }
        public IActionResult Orders()
        {
            var Id = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var user = _context.Users.Where(user => user.Id == Id).SingleOrDefault();
            ViewBag.Orders = _context.Orders.Include(order => order.User).ToList();
            return View(user);
        }
        public IActionResult ProductReviews()
        {
            var Id = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var user = _context.Users.Where(user => user.Id == Id).SingleOrDefault();
            ViewBag.Reviews = _context.Reviews.Include(review => review.Product).Include(review => review.User).ToList();
            return View(user);
        }
        public IActionResult Testimonials()
        {
            var Id = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var user = _context.Users.Where(user => user.Id == Id).SingleOrDefault();
            ViewBag.Testimonials = _context.Testimonials.Include(testimonial => testimonial.User).ToList();
            return View(user);
        }
		public IActionResult Products(int CategoryId)
		{
			var Id = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
			var user = _context.Users.Where(user => user.Id == Id).SingleOrDefault();
			ViewBag.Products = _context.Products.Include(product => product.Category).Where(product => product.CategoryId == CategoryId).ToList();
            return View(user);
        }
    }
}
