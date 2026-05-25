import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { CartItem } from '../models/cart-item.model';

export interface CartTotal {
  totalQuantity: number;
  totalPrice: number;
}

@Injectable({ providedIn: 'root' })
export class CartService {
  apiUrl = 'http://localhost:5020/api/Cart';

  constructor(private http: HttpClient) {}

  getMyCart() {
    return this.http.get<CartItem[]>(this.apiUrl);
  }

  getTotal() {
    return this.http.get<CartTotal>(`${this.apiUrl}/total`);
  }

  addToCart(medicineId: number, quantity: number = 1) {
    return this.http.post(this.apiUrl, { medicineId, quantity });
  }

  updateQuantity(id: number, quantity: number) {
    return this.http.put(`${this.apiUrl}/${id}`, { quantity });
  }

  remove(id: number) {
    return this.http.delete(`${this.apiUrl}/${id}`);
  }
}
