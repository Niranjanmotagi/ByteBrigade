import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';
import { LoyaltyService } from '../../services/loyalty.service';
import { LoyaltyTransaction } from '../../models/loyalty.model';
import { Navbar } from '../../components/navbar/navbar';

@Component({
  selector: 'app-loyalty',
  standalone: true,
  imports: [CommonModule, RouterModule, Navbar],
  templateUrl: './loyalty.html'
})
export class Loyalty implements OnInit {
  points = 0;
  transactions: LoyaltyTransaction[] = [];
  loading = true;

  constructor(private loyaltyService: LoyaltyService) {}

  ngOnInit() {
    this.loyaltyService.getPoints().subscribe((r) => (this.points = r.points));
    this.loyaltyService.getTransactions().subscribe({
      next: (t) => {
        this.transactions = t;
        this.loading = false;
      },
      error: () => (this.loading = false)
    });
  }
}
