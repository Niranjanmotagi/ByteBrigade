import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { Router, RouterModule } from '@angular/router';
import { MedicineService } from '../../services/medicine.service';
import { Medicine } from '../../models/medicine.model';
import { Navbar } from '../../components/navbar/navbar';

@Component({
  selector: 'app-add-medicine',
  standalone: true,
  imports: [CommonModule, FormsModule, RouterModule, Navbar],
  templateUrl: './add-medicine.html'
})
export class AddMedicine implements OnInit {
  med: Partial<Medicine> = this.emptyMedicine();

  medicines: Medicine[] = [];
  message = '';

  constructor(private medicineService: MedicineService, private router: Router) {}

  ngOnInit() {
    this.load();
  }

  load() {
    this.medicineService.getAll().subscribe((data) => (this.medicines = data));
  }

  emptyMedicine(): Partial<Medicine> {
    return {
      name: '',
      category: '',
      manufacturer: '',
      composition: '',
      dosageForm: '',
      strength: '',
      packagingType: '',
      price: 0,
      stockQuantity: 0,
      description: '',
      requiresPrescription: false,
      imageUrl: ''
    };
  }

  save() {
    this.medicineService.add(this.med).subscribe({
      next: () => {
        this.message = 'Medicine added';
        this.med = this.emptyMedicine();
        this.load();
        setTimeout(() => (this.message = ''), 2000);
      },
      error: (err) => (this.message = err?.error?.message || 'Failed to add')
    });
  }

  updateStock(m: Medicine, newQty: number) {
    const updated = { ...m, stockQuantity: Number(newQty) };
    this.medicineService.update(m.id, updated).subscribe(() => this.load());
  }

  delete(m: Medicine) {
    if (!confirm(`Delete ${m.name}?`)) return;
    this.medicineService.delete(m.id).subscribe(() => this.load());
  }
}
