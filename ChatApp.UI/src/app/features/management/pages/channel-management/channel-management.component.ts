import { Component, ElementRef, HostListener, OnInit } from "@angular/core";
import { ModalService } from "../../../../core/services/modal.service";
import { ChannelModalComponent } from "../../components/modals/channel-modal/channel-modal.component";

@Component({
  selector: "app-channel-management",
  standalone: true,
  imports: [],
  templateUrl: "./channel-management.component.html",
  styleUrl: "./channel-management.component.css",
})
export class ChannelManagementComponent implements OnInit {
  isServerMenuOpen: boolean = false;

  toggleServerMenu() {
    this.isServerMenuOpen = !this.isServerMenuOpen;
  }

  ngOnInit(): void {
    this.openChannelCreateModal();
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
  openChannelCreateModal() {
    this.modalService.open(ChannelModalComponent);
  }

  constructor(private elementRef: ElementRef, private modalService: ModalService) {}
}
