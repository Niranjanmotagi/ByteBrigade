import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Medicine } from '../models/medicine.model';

@Injectable({ providedIn: 'root' })
export class MedicineService {
  apiUrl = 'http://localhost:5020/api/Medicine';

  constructor(private http: HttpClient) {}

  getAll() {
    return this.http.get<Medicine[]>(this.apiUrl);
  }

  getById(id: number) {
    return this.http.get<Medicine>(`${this.apiUrl}/${id}`);
  }

  add(med: Partial<Medicine>) {
    return this.http.post<Medicine>(this.apiUrl, med);
  }

  update(id: number, med: Partial<Medicine>) {
    return this.http.put<Medicine>(`${this.apiUrl}/${id}`, med);
  }

  delete(id: number) {
    return this.http.delete(`${this.apiUrl}/${id}`);
  }
}
