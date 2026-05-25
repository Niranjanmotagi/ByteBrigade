import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';
import { OrderService } from '../../services/order.service';
import { AuthService } from '../../services/auth.service';
import { Order } from '../../models/order.model';
import { Navbar } from '../../components/navbar/navbar';

@Component({
  selector: 'app-orders',
  standalone: true,
  imports: [CommonModule, RouterModule, Navbar],
  templateUrl: './orders.html'
})
export class Orders implements OnInit {
  orders: Order[] = [];
  loading = true;
  adminMode = false;

  constructor(private orderService: OrderService, public auth: AuthService) {}

  ngOnInit() {
    this.adminMode = this.auth.isAdmin();
    this.load();
  }

  load() {
    this.loading = true;
    const call = this.adminMode
      ? this.orderService.allOrders()
      : this.orderService.myOrders();

    call.subscribe({
      next: (data) => {
        this.orders = data;
        this.loading = false;
      },
      error: () => (this.loading = false)
    });
  }

  changeStatus(o: Order, status: string) {
    this.orderService.updateStatus(o.id, status).subscribe(() => this.load());
  }

  prescriptionUrl(file?: string) {
    if (!file) return '';
    return `http://localhost:5019/prescriptions/${file}`;
  }
}
