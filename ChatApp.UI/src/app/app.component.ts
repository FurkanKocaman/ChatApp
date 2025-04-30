import { Component, OnDestroy, OnInit } from "@angular/core";
import { RouterOutlet } from "@angular/router";
import { SignalChatService } from "./core/services/signal-chat.service";
import { AuthService } from "./core/services/auth.service";
import { ToastComponent } from "./shared/components/toast/toast.component";
import { ThemeService } from "./core/services/theme.service";
import { UserService } from "./core/services/user.service";
import { MatDialogModule } from "@angular/material/dialog";
import { MatButtonModule } from "@angular/material/button";

@Component({
  selector: "app-root",
  standalone: true,
  imports: [RouterOutlet, ToastComponent, MatDialogModule, MatButtonModule],
  templateUrl: "./app.component.html",
  styleUrl: "./app.component.css",
})
export class AppComponent implements OnInit, OnDestroy {
  title = "chat_app";

  constructor(private themeService: ThemeService) {
    this.themeService.loadTheme();
  }
  ngOnInit(): void {}
  ngOnDestroy() {}
}
