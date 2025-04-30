import { Injectable } from "@angular/core";
import { BehaviorSubject } from "rxjs";
import { Toast, ToastPosition, ToastType } from "../models/entities";

@Injectable({
  providedIn: "root",
})
export class ToastService {
  private toasts: Toast[] = [];
  private toastSubject = new BehaviorSubject<Toast[]>([]);
  toast$ = this.toastSubject.asObservable();

  showToast(
    message: string,
    type: ToastType,
    duration: number = 5000,
    dismissible = true,
    position: ToastPosition = "top-right"
  ) {
    const id = Date.now();

    const newToast: Toast = { id, message, type, duration, dismissible, position };

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
