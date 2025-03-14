import { Component } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { RouterLink } from '@angular/router';
import { LoginRequest } from '../../../core/models/login-req.model';
import { AuthService } from '../../../core/services/auth.service';
import { UserService } from '../../../core/services/user.service';

@Component({
  selector: 'app-login',
  standalone: true,
  imports: [FormsModule, RouterLink],
  templateUrl: './login.component.html',
  styleUrl: 'login.component.css',
})
export class LoginComponent {
  constructor(private authService: AuthService) {}
  loginReq: LoginRequest = {
    UserNameOrEmail: '',
    Password: '',
    RememberMe: false,
  };

  isLoading: boolean = false;

  login(): void {
    this.isLoading = true;
    this.authService.login(this.loginReq).subscribe({
      next: (res) => {
        this.authService.getUserData();
        this.isLoading = false;
      },
      error: (err) => {
        console.error(err);
        this.isLoading = false;
      },
    });
  }
}
