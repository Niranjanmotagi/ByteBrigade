namespace HackathonBackend.Dtos
{
    // ===== User / Profile =====
    // Documents the contracts of user-facing profile endpoints.

    public class UserProfileDto
    {
        public int Id { get; set; }
        public string Username { get; set; } = "";
        public string Email { get; set; } = "";
        public string FirstName { get; set; } = "";
        public string LastName { get; set; } = "";
        public string PhoneNumber { get; set; } = "";
        public string Role { get; set; } = "Customer";
        public int LoyaltyPoints { get; set; }
    }

    public class UpdateUserProfileDto
    {
        public string Email { get; set; } = "";
        public string FirstName { get; set; } = "";
        public string LastName { get; set; } = "";
        public string PhoneNumber { get; set; } = "";
    }

    public class ChangePasswordDto
    {
        public string CurrentPassword { get; set; } = "";
        public string NewPassword { get; set; } = "";
    }
}
