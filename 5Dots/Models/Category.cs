using System.ComponentModel.DataAnnotations;

namespace _5Dots.Models
{
    public class Category
    {
        [Key]
        public int CategoryId { get; set; }
        [Required(ErrorMessage = "Category Name is required")]
        public string CategoryName { get; set; }
        public string CategoryDescription { get; set; }
        public List<Product> Products { get; set; }
        [Required]
        public string ImageName { get; set; }
        [Required]
        public string contentType { get; set; }
        [Required]
        public byte[] Image { get; set; }
    }
}
