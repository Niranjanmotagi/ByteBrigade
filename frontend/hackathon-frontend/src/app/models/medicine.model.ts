export interface Medicine {
  id: number;
  name: string;
  category: string;
  manufacturer: string;
  composition: string;
  dosageForm: string;
  strength: string;
  packagingType: string;
  price: number;
  stockQuantity: number;
  description: string;
  requiresPrescription: boolean;
  imageUrl: string;
}
