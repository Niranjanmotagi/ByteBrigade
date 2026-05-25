export interface Promotion {
  id: number;
  promotionCode: string;
  description: string;
  discountType: string;       // "Percentage" or "FixedAmount"
  discountValue: number;
  minimumOrderValue: number;
  maxDiscountAmount?: number | null;
  startDate: string;
  endDate: string;
  isActive: boolean;
  usageLimit?: number | null;
  usedCount: number;
}

export interface PromotionValidationResult {
  promotionId: number;
  code: string;
  description: string;
  discount: number;
}
