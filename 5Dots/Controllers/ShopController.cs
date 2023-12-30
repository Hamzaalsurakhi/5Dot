﻿using _5Dots.Data;
using _5Dots.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

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



        public IActionResult Index(string selectedCategory)
        {


            ViewBag.products = _context.Products.ToList();
            ViewBag.Categories = _context.Categories.ToList();
            ViewBag.SelectedCategory = selectedCategory;
            ViewBag.ProductImages = _context.ProductImages.ToList();


            return View();
        }
        public async Task<IActionResult> Category(int? id, string selectedCategory)
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
        public async Task<IActionResult> ProductDetails(int? id)
        {

            ViewBag.products = _context.Products.ToList();
            ViewBag.Categories = _context.Categories.ToList();
            ViewBag.Reviews = _context.Reviews.Include(review => review.Product).Include(review => review.User).Where(review => review.ProductId == id && review.ReviewStatus == "Accept").ToList();
            ViewBag.ProductImages = _context.ProductImages
            .Where(pi => pi.ProductId == id)
            .ToList();

            if (id == null || _context.Categories == null)
            {
                return NotFound();
            }

            var product = await _context.Products
                .FirstOrDefaultAsync(m => m.ProductId == id);
            if (product == null)
            {
                return NotFound();
            }
            ViewBag.UserId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value; ;
            return View(product);
        }
        public IActionResult UserCart()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            Cart cart = _context.Carts.Include(cartP => cartP.User).Where(cart => cart.UserId == userId).SingleOrDefault();
            ViewBag.Cart = cart;
            ViewBag.CartProducts = _context.CartProducts.Include(cartP => cartP.Product).ThenInclude(product => product.Category).Where(cartP => cartP.CartId == cart.CartId).ToList();
            return View();

        }
        public async Task<IActionResult> RemoveProductFromCart(int ProductId,int CartId)
        {
            Product product = _context.Products.Where(product => product.ProductId == ProductId).SingleOrDefault();
            CartProduct cartProduct = _context.CartProducts.Include(cartP=>cartP.Product).Where(cartP => cartP.ProductId == ProductId && cartP.CartId == CartId).SingleOrDefault();
            Cart cart = _context.Carts.Where(cart => cart.CartId == CartId).SingleOrDefault();
            cart.TotalQuantity--;
            cart.TotalPrice -= cartProduct.ProductQuantity * cartProduct.Product.ProductPrice;
            product.ProductQuantityStock += cartProduct.ProductQuantity;
            _context.Remove(cartProduct);
            await _context.SaveChangesAsync();
            _context.Update(product);
            await _context.SaveChangesAsync();
            _context.Update(cart);
            await _context.SaveChangesAsync();
            return RedirectToAction("UserCart");
        }

        public async Task<IActionResult> RemoveOneItemFromProduct(int ProductId, int CartId)
        {
            Product product = _context.Products.Where(product => product.ProductId == ProductId).SingleOrDefault();
            CartProduct cartProduct = _context.CartProducts.Include(cartP => cartP.Product).Where(cartP => cartP.ProductId == ProductId && cartP.CartId == CartId).SingleOrDefault();
            Cart cart = _context.Carts.Where(cart => cart.CartId == CartId).SingleOrDefault();

            if (cartProduct.ProductQuantity > 1)
            {
                cartProduct.ProductQuantity--;
                cart.TotalPrice -= product.ProductPrice;
                product.ProductQuantityStock++;
                _context.Update(cartProduct);
                await _context.SaveChangesAsync();
                _context.Update(cart);
                await _context.SaveChangesAsync();
                _context.Update(product);
                await _context.SaveChangesAsync();
            }
            else if(cartProduct.ProductQuantity == 1) {
                cart.TotalQuantity--;
                cart.TotalPrice -= cartProduct.Product.ProductPrice;
                product.ProductQuantityStock++;
                _context.Remove(cartProduct);
                await _context.SaveChangesAsync();
                _context.Update(product);
                await _context.SaveChangesAsync();
                _context.Update(cart);
                await _context.SaveChangesAsync();
            }
 
            return RedirectToAction("UserCart");
        }
        public async Task<IActionResult> AddOneItemToProduct(int ProductId, int CartId)
        {
            Product product = _context.Products.Where(product => product.ProductId == ProductId).SingleOrDefault();
            CartProduct cartProduct = _context.CartProducts.Include(cartP => cartP.Product).Where(cartP => cartP.ProductId == ProductId && cartP.CartId == CartId).SingleOrDefault();
            Cart cart = _context.Carts.Where(cart => cart.CartId == CartId).SingleOrDefault();
            cartProduct.ProductQuantity++;
            cart.TotalPrice += product.ProductPrice;
            product.ProductQuantityStock--;
            _context.Update(cartProduct);
            await _context.SaveChangesAsync();
            _context.Update(cart);
            await _context.SaveChangesAsync();
            _context.Update(product);
            await _context.SaveChangesAsync();
            return RedirectToAction("UserCart");
        }
        public IActionResult Checkout()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            ViewBag.Cart = _context.Carts.Include(cartP => cartP.User).Where(cart => cart.UserId == userId).SingleOrDefault();
            ViewBag.Visa = _context.Visa.Where(visa => visa.UserId == userId).SingleOrDefault();
            User user = _context.Users.Where(user=>user.Id == userId).SingleOrDefault();
            return View(user);
        }
        public async Task<IActionResult> Proceed()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            Cart cart = _context.Carts.Where(cart => cart.UserId == userId).SingleOrDefault();
            List<CartProduct> cartProducts = _context.CartProducts.Where(cartP => cartP.CartId == cart.CartId).ToList();
            Order order = new Order();
            order.TotalPrice = cart.TotalPrice;
            order.UserId = userId;
            order.Status = "Pending";
            order.OrderRate = 0;
            _context.Add(order);
            await _context.SaveChangesAsync();
            foreach (var cartProduct in cartProducts)
            {
                OrderProduct orderProduct = new OrderProduct();
                orderProduct.ProductId = cartProduct.ProductId;
                orderProduct.OrderId= order.OrderId;
                orderProduct.ProductQuantity = cartProduct.ProductQuantity;
                _context.Add(orderProduct);
                await _context.SaveChangesAsync();
                _context.Remove(cartProduct);
                await _context.SaveChangesAsync();
            }
            Payment payment = new Payment();
            payment.OrderId = order.OrderId;
            payment.Amount = order.TotalPrice;
            payment.TransactionDate = DateTime.Now;
            _context.Add(payment);
            await _context.SaveChangesAsync();
            cart.TotalPrice = 0;
            cart.TotalQuantity= 0;
            _context.Update(cart);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index","Home");
        }
        public IActionResult AddVisa()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateVisa(Visa visa)
        {
            visa.UserId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            _context.Add(visa);
            await _context.SaveChangesAsync();
            return RedirectToAction("Checkout", "Shop");
        }
    }
}
