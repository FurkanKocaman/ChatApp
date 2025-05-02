import { CommonModule } from "@angular/common";
import { Component, ElementRef, HostListener } from "@angular/core";

@Component({
  selector: "app-member-management",
  standalone: true,
  imports: [CommonModule],
  templateUrl: "./member-management.component.html",
  styleUrl: "./member-management.component.css",
})
export class MemberManagementComponent {
  isServerMenuOpen: boolean = false;

  toggleServerMenu() {
    this.isServerMenuOpen = !this.isServerMenuOpen;
  }
  @HostListener("document:click", ["$event"])
  onDocumentClick(event: MouseEvent) {
    if (!event.target || !this.isServerMenuOpen) return;

    const clickedInside = this.elementRef.nativeElement.contains(event.target);
    if (!clickedInside) {
      this.isServerMenuOpen = false;
    }
  }
  @HostListener("document:keydown", ["$event"])
  handleKeyboardEvent(event: KeyboardEvent) {
    if (event.key === "Escape" && this.isServerMenuOpen) {
      this.isServerMenuOpen = false;
    }
  }

  constructor(private elementRef: ElementRef) {}
}
