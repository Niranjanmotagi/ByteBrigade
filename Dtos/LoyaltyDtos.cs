namespace HackathonBackend.Dtos
{
    // ===== Loyalty =====
    // Documents the contracts of /api/Loyalty endpoints.

    public class LoyaltyPointsDto
    {
        public int Points { get; set; }
    }

    public class LoyaltyTransactionDto
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int? OrderId { get; set; }
        public int PointsEarned { get; set; }
        public int PointsRedeemed { get; set; }
        public DateTime TransactionDate { get; set; }
        public string Description { get; set; } = "";
    }

    public class RedeemLoyaltyDto
    {
        public int Points { get; set; }
    }
}
