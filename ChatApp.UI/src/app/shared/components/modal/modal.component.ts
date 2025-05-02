// modal.component.ts
import { Component, Output, EventEmitter, ViewChild } from "@angular/core";
import { ViewContainerRef } from "@angular/core";

@Component({
  selector: "app-modal",
  template: `
    <div class="modal-overlay" (click)="close()">
      <div class="modal-content" (click)="$event.stopPropagation()">
        <ng-container #modalContent></ng-container>
      </div>
    </div>
  `,
  styleUrls: ["./modal.component.css"],
})
export class ModalComponent {
  @Output() closeEvent = new EventEmitter<void>();
  @ViewChild("modalContent", { read: ViewContainerRef, static: true })
  modalContent!: ViewContainerRef;

  close() {
    this.closeEvent.emit();
  }
}
