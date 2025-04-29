import { Component } from "@angular/core";
import { FormsModule } from "@angular/forms";
import { RouterLink } from "@angular/router";
import { LoginRequest } from "../../../core/models/requests/login-request.model";
import { AuthService } from "../../../core/services/auth.service";
import { ThemeService } from "../../../core/services/theme.service";

@Component({
  selector: "app-login",
  standalone: true,
  imports: [FormsModule, RouterLink],
  templateUrl: "./login.component.html",
  styleUrl: "login.component.css",
})
export class LoginComponent {
  constructor(private authService: AuthService, private themeService: ThemeService) {}
  loginReq: LoginRequest = {
    UserNameOrEmail: "",
    Password: "",
    RememberMe: false,
  };

  isLoading: boolean = false;
  isDark: boolean = this.themeService.isDarkMode();

  login(): void {
    this.isLoading = true;
    this.authService.login(this.loginReq).subscribe({
      next: (res) => {
        console.log(res);
        // this.authService.getUserData();
        this.isLoading = false;
      },
      error: (err) => {
        console.error(err);
        this.isLoading = false;
      },
    });
  }

  toggleTheme(): void {
    this.themeService.toggleTheme();
    this.isDark = this.themeService.isDarkMode();
  }
}
