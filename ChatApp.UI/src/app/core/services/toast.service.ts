import { Injectable } from '@angular/core';
import { ToastMessage } from '../models/toast-message';
import { BehaviorSubject } from 'rxjs';

@Injectable({
  providedIn: 'root',
})
export class ToastService {
  private toasts: ToastMessage[] = [];
  private toastSubject = new BehaviorSubject<ToastMessage[]>([]);
  toast$ = this.toastSubject.asObservable();

  showToast(
    message: string,
    type: 'success' | 'error' | 'info' | 'warning',
    duration: number = 5000
  ) {
    const id = Date.now();

    const newToast: ToastMessage = { id, message, type, duration };

    this.toasts.push(newToast);
    this.toastSubject.next([...this.toasts]);

    setTimeout(() => {
      this.removeToast(id);
    }, duration);
  }

  removeToast(id: number) {
    this.toasts = this.toasts.filter((toast) => toast.id != id);
    this.toastSubject.next([...this.toasts]);
  }
}
