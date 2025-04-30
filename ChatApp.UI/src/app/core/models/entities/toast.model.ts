import { TemplateRef } from "@angular/core";

export type ToastType = "success" | "error" | "info" | "warning" | "custom";
export type ToastPosition =
  | "top-left"
  | "top-center"
  | "top-right"
  | "bottom-left"
  | "bottom-center"
  | "bottom-right";

export interface Toast {
  id: number;
  message?: string;
  type: ToastType;
  duration?: number;
  dismissible: boolean;
  position: ToastPosition;
}
