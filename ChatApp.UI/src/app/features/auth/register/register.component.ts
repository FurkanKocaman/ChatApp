import { Component } from "@angular/core";
import { RouterLink } from "@angular/router";
import { RegisterRequest } from "../../../core/models/requests/register-request.model";
import { FormsModule } from "@angular/forms";
import { ThemeService } from "../../../core/services/theme.service";
import { AuthService } from "../../../core/services/auth.service";

@Component({
  selector: "app-register",
  standalone: true,
  imports: [RouterLink, FormsModule],
  templateUrl: "./register.component.html",
  styleUrl: "./register.component.css",
})
export class RegisterComponent {
  constructor(private themeService: ThemeService, private authService: AuthService) {}
  registerReq: RegisterRequest = {
    firstName: "",
    lastName: "",
    userName: "",
    email: "",
    password: "",
    birthOfDate: new Date(),
    gender: undefined,
  };

  isDark: boolean = this.themeService.isDarkMode();

  register(): void {
    this.authService.register(this.registerReq).subscribe((res) => {
      console.log(res);
    });
  }

  toggleTheme(): void {
    this.themeService.toggleTheme();
    this.isDark = this.themeService.isDarkMode();
  }
}
