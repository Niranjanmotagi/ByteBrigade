import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { Router, RouterModule } from '@angular/router';
import { CartService } from '../../services/cart.service';
import { OrderService } from '../../services/order.service';
import { CartItem } from '../../models/cart-item.model';
import { Navbar } from '../../components/navbar/navbar';

@Component({
  selector: 'app-cart',
  standalone: true,
  imports: [CommonModule, FormsModule, RouterModule, Navbar],
  templateUrl: './cart.html'
})
export class Cart implements OnInit {
  items: CartItem[] = [];
  loading = true;
  updatingId: number | null = null;
  purchasing = false;
  error = '';

  // Prescription upload (only used when needsPrescription())
  selectedFile: File | null = null;

  constructor(
    private cartService: CartService,
    private orderService: OrderService,
    private router: Router
  ) {}

  ngOnInit() {
    this.load();
  }

  load(clearError = true) {
    this.loading = true;
    if (clearError) this.error = '';
    this.cartService.getMyCart().subscribe({
      next: (data) => {
        this.items = data;
        this.loading = false;
        this.updatingId = null;
      },
      error: () => {
        this.loading = false;
        this.updatingId = null;
        this.error = 'Unable to load your cart.';
      }
    });
  }

  remove(item: CartItem) {
    this.cartService.remove(item.id).subscribe(() => this.load());
  }

  updateQuantity(item: CartItem, quantityValue: number | string) {
    const quantity = Math.trunc(Number(quantityValue));
    if (!Number.isFinite(quantity) || quantity < 1) {
      this.error = 'Quantity must be at least 1.';
      return;
    }

    const stock = item.medicine?.stockQuantity ?? quantity;
    if (quantity > stock) {
      this.error = `Only ${stock} item(s) available in stock.`;
      return;
    }

    if (quantity === item.quantity) return;

    this.error = '';
    this.updatingId = item.id;
    this.cartService.updateQuantity(item.id, quantity).subscribe({
      next: () => this.load(),
      error: (err) => {
        this.updatingId = null;
        this.error = err?.error?.message || 'Unable to update quantity.';
        this.load(false);
      }
    });
  }

  lineTotal(item: CartItem): number {
    return item.totalPrice ?? (item.medicine?.price || 0) * item.quantity;
  }

  total(): number {
    return this.items.reduce((sum, i) => sum + this.lineTotal(i), 0);
  }

  needsPrescription(): boolean {
    return this.items.some((i) => i.medicine?.requiresPrescription);
  }

  onFile(event: any) {
    const file: File | undefined = event?.target?.files?.[0];
    this.error = '';

    if (!file) {
      this.selectedFile = null;
      return;
    }

    // Only allow JPG / PNG for cart-side prescription uploads
    const name = (file.name || '').toLowerCase();
    const type = (file.type || '').toLowerCase();
    const isJpgOrPng =
      type === 'image/jpeg' ||
      type === 'image/png' ||
      name.endsWith('.jpg') ||
      name.endsWith('.jpeg') ||
      name.endsWith('.png');

    if (!isJpgOrPng) {
      this.selectedFile = null;
      // Reset the input so the user can re-pick the same file after fixing
      try { event.target.value = ''; } catch {}
      this.error = 'Only JPG or PNG images are allowed for the prescription.';
      return;
    }

    this.selectedFile = file;
  }

  purchase() {
    if (this.purchasing || this.items.length === 0) return;

    if (this.needsPrescription() && !this.selectedFile) {
      this.error =
        'Some items in your cart require a prescription. Please upload it (JPG / PNG / PDF) before purchasing.';
      return;
    }

    this.purchasing = true;
    this.error = '';

    // Place the order on the backend (saves order, reduces stock, clears cart,
    // awards loyalty points, sends confirmation email).
    this.orderService
      .placeOrder({
        prescription: this.selectedFile,
        // Cart-page quick purchase uses sensible defaults; full checkout page
        // collects address / phone / payment for a complete flow.
        address: 'To be collected at delivery',
        phone: '0000000000',
        notes: 'Quick purchase from cart'
      })
      .subscribe({
        next: () => {
          this.purchasing = false;
          this.items = [];
          this.selectedFile = null;
          alert('Order confirmed — waiting for validation');
          this.router.navigate(['/orders']);
        },
        error: (err) => {
          console.error(err);
          this.purchasing = false;
          this.error =
            err?.error?.message ||
            'Could not place the order. Please try again.';
        }
      });
  }
}
