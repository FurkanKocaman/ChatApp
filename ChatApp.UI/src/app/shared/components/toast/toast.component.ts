import { Component, OnInit } from '@angular/core';
import { ToastMessage } from '../../../core/models/toast-message';
import { ToastService } from '../../../core/services/toast.service';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-toast',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './toast.component.html',
  styleUrl: './toast.component.css',
})
export class ToastComponent {
  toasts: ToastMessage[] = [];

  constructor(private toastService: ToastService) {
    this.toastService.toast$.subscribe((messages) => {
      this.toasts = messages;
    });
  }

  removeToast(id: number) {
    this.toastService.removeToast(id);
  }
}
