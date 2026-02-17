using System.ComponentModel.DataAnnotations;

namespace TaskWebApi.Models
{
    public class Product
    {
        [Key]
        public long Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public bool IsActive { get; set; } = true;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
