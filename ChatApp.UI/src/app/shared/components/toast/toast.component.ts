import { Component, OnInit } from "@angular/core";
import { ToastService } from "../../../core/services/toast.service";
import { CommonModule } from "@angular/common";
import { Toast } from "../../../core/models/entities";

@Component({
  selector: "app-toast",
  standalone: true,
  imports: [CommonModule],
  templateUrl: "./toast.component.html",
  styleUrl: "./toast.component.css",
})
export class ToastComponent {
  toasts: Toast[] = [];

  constructor(private toastService: ToastService) {
    this.toastService.toast$.subscribe((messages) => {
      this.toasts = messages;
    });
  }

  removeToast(id: number) {
    this.toastService.removeToast(id);
  }
}
