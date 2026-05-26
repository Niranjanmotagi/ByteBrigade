import { Medicine } from './medicine.model';

export interface CartItem {
  id: number;
  userId: number;
  medicineId: number;
  quantity: number;
  totalPrice?: number;
  medicine?: Medicine;
}
