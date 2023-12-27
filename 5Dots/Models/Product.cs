using System.ComponentModel.DataAnnotations;

namespace _5Dots.Models
{
    public class Product
    {
        [Key]
        public int ProductId { get; set; }

        [Required(ErrorMessage = "Product name is required")]
        public string ProductName { get; set; }

        [Required(ErrorMessage = "Product Description is required")]
        [StringLength(255, ErrorMessage = "Product description cannot exceed 255 characters")]
        public string ProductDescription { get; set; }

        [Required(ErrorMessage = "Product price is required")]
        [Range(0.01, double.MaxValue, ErrorMessage = "Product price must be greater than 0")]
        public decimal ProductPrice { get; set; }

        [Required(ErrorMessage = "Product quantity in stock is required")]
        [Range(0, int.MaxValue, ErrorMessage = "Product quantity in stock must be greater than or equal to 0")]
        public int ProductQuantityStock { get; set; }

        [Range(0, 100, ErrorMessage = "Product sale percentage must be between 0 and 100")]
        public decimal ProductSale { get; set; }

        [Required(ErrorMessage = "Category ID is required")]
        public int CategoryId { get; set; }

        [Required(ErrorMessage = "Category is required")]
        public Category Category { get; set; }

        [Required]
        public string ImageName { get; set; }
        [Required]
        public string contentType { get; set; }
        [Required]
        public byte[] Image { get; set; }

        //public List<Order> Orders { get; set; }
    }
}