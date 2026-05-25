import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { Router, RouterModule } from '@angular/router';
import { timeout, catchError, of } from 'rxjs';

import { CartService } from '../../services/cart.service';
import { OrderService } from '../../services/order.service';
import { PromotionService } from '../../services/promotion.service';

import { CartItem } from '../../models/cart-item.model';

import { Navbar } from '../../components/navbar/navbar';

@Component({
  selector: 'app-checkout',
  standalone: true,
  imports: [CommonModule, FormsModule, RouterModule, Navbar],
  templateUrl: './checkout.html'
})
export class Checkout implements OnInit {
  items: CartItem[] = [];

  loading = true;
  placing = false;
  loadError = '';
  error = '';
  successMessage = '';

  // Prescription
  selectedFile: File | null = null;

  // Promo
  promoCode = '';
  promoMessage = '';
  promoDiscount = 0;
  promoValid = false;

  // Delivery details
  address = '';
  phone = '';
  notes = '';

  // Payment
  paymentMethod: 'COD' | 'UPI' | 'CARD' = 'COD';
  upiId = '';
  cardNumber = '';
  cardExpiry = '';
  cardCvv = '';

  constructor(
    private cartService: CartService,
    private orderService: OrderService,
    private promotionService: PromotionService,
    private router: Router
  ) {}

  ngOnInit() {
    this.loadCart();
  }

  loadCart() {
    this.loading = true;
    this.loadError = '';

    this.cartService
      .getMyCart()
      .pipe(
        // Don't let the spinner hang forever if the backend is unreachable
        timeout(8000),
        catchError((err) => {
          console.error('[checkout] failed to load cart', err);
          this.loadError =
            err?.status === 401
              ? 'Your session expired. Please log in again.'
              : err?.name === 'TimeoutError'
              ? 'The server is taking too long to respond. Is the backend running?'
              : err?.error?.message ||
                'Could not load your cart. Please try again.';
          return of([] as CartItem[]);
        })
      )
      .subscribe((data) => {
        this.items = data || [];
        this.loading = false;

        if (this.loadError && this.loadError.startsWith('Your session')) {
          // Hard redirect to login if 401
          setTimeout(() => this.router.navigate(['/login']), 1200);
        }
      });
  }

  needsPrescription(): boolean {
    return this.items.some((i) => i.medicine?.requiresPrescription);
  }

  lineTotal(item: CartItem): number {
    return (
      item.totalPrice ??
      (item.medicine?.price || 0) * item.quantity
    );
  }

  subtotal(): number {
    return this.items.reduce((sum, i) => sum + this.lineTotal(i), 0);
  }

  finalTotal(): number {
    return Math.max(0, this.subtotal() - this.promoDiscount);
  }

  onFile(event: any) {
    const file = event?.target?.files?.[0];
    this.selectedFile = file || null;
  }

  applyPromo() {
    this.promoMessage = '';

    if (!this.promoCode.trim()) {
      this.promoDiscount = 0;
      this.promoValid = false;
      return;
    }

    this.promotionService
      .validate(this.promoCode.trim(), this.subtotal())
      .subscribe({
        next: (res) => {
          this.promoDiscount = res.discount;
          this.promoValid = true;
          this.promoMessage = `${res.description} — ₹${res.discount} off`;
        },
        error: (err) => {
          this.promoDiscount = 0;
          this.promoValid = false;
          this.promoMessage = err?.error?.message || 'Invalid promo code';
        }
      });
  }

  validatePayment(): string | null {
    if (this.paymentMethod === 'COD') return null;

    if (this.paymentMethod === 'UPI') {
      if (!this.upiId.trim() || !this.upiId.includes('@')) {
        return 'Enter a valid UPI ID (e.g. name@bank).';
      }
      return null;
    }

    if (this.paymentMethod === 'CARD') {
      const cleanCard = this.cardNumber.replace(/\s+/g, '');
      if (!/^\d{12,19}$/.test(cleanCard)) {
        return 'Enter a valid card number.';
      }
      if (!/^\d{2}\/\d{2}$/.test(this.cardExpiry)) {
        return 'Card expiry must be in MM/YY format.';
      }
      if (!/^\d{3,4}$/.test(this.cardCvv)) {
        return 'CVV must be 3 or 4 digits.';
      }
      return null;
    }

    return null;
  }

  placeOrder() {
    this.error = '';
    this.successMessage = '';

    if (!this.address.trim()) {
      this.error = 'Please enter a delivery address.';
      return;
    }
    if (!/^\d{10}$/.test(this.phone.trim())) {
      this.error = 'Please enter a valid 10-digit phone number.';
      return;
    }
    if (this.needsPrescription() && !this.selectedFile) {
      this.error = 'Please upload prescription before placing the order.';
      return;
    }

    const paymentError = this.validatePayment();
    if (paymentError) {
      this.error = paymentError;
      return;
    }

    this.placing = true;

    const notesWithPayment = [
      this.notes.trim(),
      `Payment: ${this.paymentMethod}` +
        (this.paymentMethod === 'UPI'
          ? ` (${this.upiId})`
          : this.paymentMethod === 'CARD'
          ? ` (xxxx-${this.cardNumber.slice(-4)})`
          : '')
    ]
      .filter(Boolean)
      .join(' | ');

    this.orderService
      .placeOrder({
        prescription: this.selectedFile,
        promoCode: this.promoValid ? this.promoCode.trim() : undefined,
        address: this.address.trim(),
        phone: this.phone.trim(),
        notes: notesWithPayment
      })
      .subscribe({
        next: (res) => {
          this.placing = false;
          this.successMessage = `Order placed! ${res.orderNumber} — ₹${res.finalAmount} • ${res.pointsEarned} points earned.`;
          setTimeout(() => this.router.navigate(['/orders']), 1500);
        },
        error: (err) => {
          console.error(err);
          this.placing = false;
          this.error = err?.error?.message || 'Failed to place order';
        }
      });
  }
}
