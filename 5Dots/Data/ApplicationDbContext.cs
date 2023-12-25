using _5Dots.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace _5Dots.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        DbSet<Cart> Carts { get; set; }
        DbSet<CartProduct> CartProducts { get; set; }
        DbSet<Category> Categories { get; set; }
        DbSet<Order> Orders { get; set; }
        DbSet<Payment> Payments { get; set; }
        DbSet<Product> Products { get; set; }
        DbSet<ProductImage> ProductImages { get; set; }
        DbSet<Review> Reviews { get; set; }
        DbSet<Testimonial> Testimonials { get; set; }
        DbSet<Visa> Visa { get; set; }
        DbSet<User> Users { get; set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
    }
}
