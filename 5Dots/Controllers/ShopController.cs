using _5Dots.Data;
using _5Dots.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace _5Dots.Controllers
{
    public class ShopController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDbContext _context;
        private readonly RoleManager<IdentityRole> _roleManager;
        public ShopController(ApplicationDbContext context, RoleManager<IdentityRole> roleManager)
        {
            _context = context;
            _roleManager = roleManager;
        }



        public  IActionResult Index(string selectedCategory)
        {
            

            ViewBag.products = _context.Products.ToList();
            ViewBag.Categories = _context.Categories.ToList();
            ViewBag.SelectedCategory = selectedCategory;
            ViewBag.ProductImages = _context.ProductImages.ToList();
            

            return View();
        }
        public async Task<IActionResult> Category(int? id,string selectedCategory)
        {

            ViewBag.products = _context.Products.ToList();
            ViewBag.Categories = _context.Categories.ToList();
            ViewBag.SelectedCategory = selectedCategory;
            ViewBag.ProductImages = _context.ProductImages.ToList();
            if (id == null || _context.Categories == null)
            {
                return NotFound();
            }

            var category = await _context.Categories
                .FirstOrDefaultAsync(m => m.CategoryId == id);
            if (category == null)
            {
                return NotFound();
            }

            return View(category);
        }
    }
    
}
