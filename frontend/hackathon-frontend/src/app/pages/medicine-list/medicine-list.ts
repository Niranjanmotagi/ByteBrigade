import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { Router, RouterModule } from '@angular/router';
import { MedicineService } from '../../services/medicine.service';
import { CartService } from '../../services/cart.service';
import { AuthService } from '../../services/auth.service';
import { Medicine } from '../../models/medicine.model';
import { Navbar } from '../../components/navbar/navbar';

@Component({
  selector: 'app-medicine-list',
  standalone: true,
  imports: [CommonModule, FormsModule, RouterModule, Navbar],
  templateUrl: './medicine-list.html'
})
export class MedicineList implements OnInit {
  medicines: Medicine[] = [];
  search = '';
  selectedCategory = '';
  selectedDosage = '';
  selectedPackaging = '';
  loading = true;
  message = '';
  showDetailsId: number | null = null;

  constructor(
    private medicineService: MedicineService,
    private cartService: CartService,
    public auth: AuthService,
    private router: Router
  ) {}

  ngOnInit() {
    this.load();
  }

  load() {
    this.loading = true;
    this.medicineService.getAll().subscribe({
      next: (data) => {
        this.medicines = data;
        this.loading = false;
      },
      error: () => (this.loading = false)
    });
  }

  filtered(): Medicine[] {
    const q = this.search.toLowerCase().trim();
    return this.medicines.filter((m) => {
      const matchesSearch =
        !q ||
        m.name.toLowerCase().includes(q) ||
        m.category.toLowerCase().includes(q) ||
        (m.composition || '').toLowerCase().includes(q);

      const matchesCategory =
        !this.selectedCategory || m.category === this.selectedCategory;
      const matchesDosage =
        !this.selectedDosage || m.dosageForm === this.selectedDosage;
      const matchesPackaging =
        !this.selectedPackaging || m.packagingType === this.selectedPackaging;

      return (
        matchesSearch && matchesCategory && matchesDosage && matchesPackaging
      );
    });
  }

  categories(): string[] {
    return Array.from(new Set(this.medicines.map((m) => m.category))).sort();
  }

  dosageForms(): string[] {
    return Array.from(new Set(this.medicines.map((m) => m.dosageForm))).sort();
  }

  packagingTypes(): string[] {
    return Array.from(
      new Set(this.medicines.map((m) => m.packagingType))
    ).sort();
  }

  resetFilters() {
    this.search = '';
    this.selectedCategory = '';
    this.selectedDosage = '';
    this.selectedPackaging = '';
  }

  toggleDetails(id: number) {
    this.showDetailsId = this.showDetailsId === id ? null : id;
  }

  addToCart(med: Medicine) {
    if (!this.auth.isLoggedIn()) {
      this.router.navigate(['/login']);
      return;
    }
    this.cartService.addToCart(med.id, 1).subscribe({
      next: () => (this.message = `${med.name} added to cart`),
      error: (err) => (this.message = err?.error?.message || 'Failed to add')
    });
    setTimeout(() => (this.message = ''), 2500);
  }

  delete(med: Medicine) {
    if (!confirm(`Delete ${med.name}?`)) return;
    this.medicineService.delete(med.id).subscribe(() => this.load());
  }
}
