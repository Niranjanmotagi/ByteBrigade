using Microsoft.AspNetCore.Http;

namespace HackathonBackend.Models
{
    public class PlaceOrderRequest
    {
        public IFormFile? Prescription { get; set; }

        public string? PromoCode { get; set; }

        public string? Address { get; set; }

        public string? Phone { get; set; }

        public string? Notes { get; set; }
    }
}