import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { LoyaltyTransaction } from '../models/loyalty.model';

@Injectable({ providedIn: 'root' })
export class LoyaltyService {
  apiUrl = 'http://localhost:5019/api/Loyalty';

  constructor(private http: HttpClient) {}

  getPoints() {
    return this.http.get<{ points: number }>(`${this.apiUrl}/points`);
  }

  getTransactions() {
    return this.http.get<LoyaltyTransaction[]>(`${this.apiUrl}/transactions`);
  }
}
