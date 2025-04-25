import { Component } from "@angular/core";
import { RouterLink } from "@angular/router";
import { RegisterRequest } from "../../../core/models/register-req.model";
import { CommonModule } from "@angular/common";
import { FormsModule } from "@angular/forms";
import { ThemeService } from "../../../core/services/theme.service";

@Component({
  selector: "app-register",
  standalone: true,
  imports: [RouterLink, FormsModule],
  templateUrl: "./register.component.html",
  styleUrl: "./register.component.css",
})
export class RegisterComponent {
  constructor(private themeService: ThemeService) {}
  registerReq: RegisterRequest = {
    firstname: "",
    lastname: "",
    username: "",
    email: "",
    password: "",
  };

  isDark: boolean = this.themeService.isDarkMode();

  register(): void {}

  toggleTheme(): void {
    this.themeService.toggleTheme();
    this.isDark = this.themeService.isDarkMode();
  }
}
