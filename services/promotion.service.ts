import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Promotion, PromotionValidationResult } from '../models/promotion.model';

@Injectable({ providedIn: 'root' })
export class PromotionService {
  apiUrl = 'http://localhost:5020/api/Promotion';

  constructor(private http: HttpClient) {}

  getActive() {
    return this.http.get<Promotion[]>(this.apiUrl);
  }

  validate(code: string, orderAmount: number) {
    return this.http.post<PromotionValidationResult>(`${this.apiUrl}/validate`, {
      code,
      orderAmount
    });
  }
}
