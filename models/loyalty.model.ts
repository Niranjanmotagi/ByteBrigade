export interface LoyaltyTransaction {
  id: number;
  userId: number;
  orderId?: number | null;
  pointsEarned: number;
  pointsRedeemed: number;
  transactionDate: string;
  description: string;
}
