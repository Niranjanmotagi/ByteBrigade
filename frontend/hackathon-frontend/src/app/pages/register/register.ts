import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { Router, RouterModule } from '@angular/router';
import { AuthService } from '../../services/auth.service';
import { Navbar } from '../../components/navbar/navbar';

@Component({
  selector: 'app-register',
  standalone: true,
  imports: [CommonModule, FormsModule, RouterModule, Navbar],
  templateUrl: './register.html'
})
export class Register {
  username = '';
  password = '';
  email = '';
  firstName = '';
  lastName = '';
  phoneNumber = '';
  error = '';
  success = '';

  constructor(private auth: AuthService, private router: Router) {}

  register() {
    this.error = '';
    this.success = '';
    this.auth
      .register({
        username: this.username,
        password: this.password,
        role: 'Customer',
        email: this.email,
        firstName: this.firstName,
        lastName: this.lastName,
        phoneNumber: this.phoneNumber
      })
      .subscribe({
        next: () => {
          this.success = 'Account created. You can now log in.';
          setTimeout(() => this.router.navigate(['/login']), 800);
        },
        error: (err) => {
          this.error = err?.error?.message || 'Registration failed';
        }
      });
  }
}
