using System.ComponentModel.DataAnnotations.Schema;

namespace HackathonBackend.Models
{
    public class CartItem
    {
        public int Id { get; set; }

        public int UserId { get; set; }

        public int MedicineId { get; set; }

        public int Quantity { get; set; }

        [NotMapped]
        public decimal TotalPrice => (Medicine?.Price ?? 0m) * Quantity;

        // Navigation (not required, but handy for joins)
        public Medicine? Medicine { get; set; }
    }
}
