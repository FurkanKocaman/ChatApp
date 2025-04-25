import { Component } from "@angular/core";
import { ThemeService } from "../../core/services/theme.service";
import { FormsModule } from "@angular/forms";
import { CommonModule } from "@angular/common";

@Component({
  selector: "app-chat-layout",
  standalone: true,
  imports: [FormsModule, CommonModule],
  templateUrl: "./chat-layout.component.html",
  styleUrl: "./chat-layout.component.css",
})
export class ChatLayoutComponent {
  constructor(private themeService: ThemeService) {}
  isDark: boolean = this.themeService.isDarkMode();

  text: string = "";

  toggleTheme(): void {
    this.themeService.toggleTheme();
    this.isDark = this.themeService.isDarkMode();
  }
}
