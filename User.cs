namespace HackathonBackend.Models
{
    public class User
    {
        public int Id { get; set; }

        public string Username { get; set; } = "";

        public string Password { get; set; } = ""; // plain text for hackathon

        // "Admin" or "Customer" (no separate Roles table per hackathon rules)
        public string Role { get; set; } = "Customer";

        // ---- SRS profile fields ----
        public string Email { get; set; } = "";
        public string FirstName { get; set; } = "";
        public string LastName { get; set; } = "";
        public string PhoneNumber { get; set; } = "";

        // Stretch: loyalty
        public int LoyaltyPoints { get; set; } = 0;
    }
}
