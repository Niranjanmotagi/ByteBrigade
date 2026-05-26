namespace HackathonBackend.Models
{
    public class LoyaltyTransaction
    {
        public int Id { get; set; }

        public int UserId { get; set; }
        public int? OrderId { get; set; }

        public int PointsEarned { get; set; } = 0;
        public int PointsRedeemed { get; set; } = 0;

        public DateTime TransactionDate { get; set; } = DateTime.UtcNow;
        public string Description { get; set; } = "";
    }
}
