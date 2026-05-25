import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Order } from '../models/order.model';

export interface PlaceOrderResponse {
  message: string;
  orderId: number;
  orderNumber: string;
  totalAmount: number;
  discountAmount: number;
  finalAmount: number;
  estimatedDeliveryDate?: string | null;
  pointsEarned: number;
}

export interface PlaceOrderPayload {
  prescription?: File | null;
  promoCode?: string;
  address?: string;
  phone?: string;
  notes?: string;
}

@Injectable({ providedIn: 'root' })
export class OrderService {
  apiUrl = 'http://localhost:5020/api/Order';

  constructor(private http: HttpClient) {}

  placeOrder(payload: PlaceOrderPayload) {
    const form = new FormData();
    if (payload.prescription) form.append('prescription', payload.prescription);
    if (payload.promoCode) form.append('promoCode', payload.promoCode);
    if (payload.address) form.append('address', payload.address);
    if (payload.phone) form.append('phone', payload.phone);
    if (payload.notes) form.append('notes', payload.notes);

    return this.http.post<PlaceOrderResponse>(`${this.apiUrl}/place`, form);
  }

  myOrders() {
    return this.http.get<Order[]>(`${this.apiUrl}/my`);
  }

  allOrders() {
    return this.http.get<Order[]>(`${this.apiUrl}/all`);
  }

  updateStatus(id: number, status: string) {
    return this.http.put<Order>(`${this.apiUrl}/${id}/status`, JSON.stringify(status), {
      headers: { 'Content-Type': 'application/json' }
    });
  }
}
