import { Component, EventEmitter, Input, Output } from "@angular/core";
import { ServerCreateRequest } from "../../../../core/models/requests";
import { ServerService } from "../../../../core/services/server.service";
import { FormsModule } from "@angular/forms";

@Component({
  selector: "app-server-create-modal",
  standalone: true,
  imports: [FormsModule],
  templateUrl: "./server-create-modal.component.html",
  styleUrl: "./server-create-modal.component.css",
})
export class ServerCreateModalComponent {
  @Input() isServerCreateModalOpen: boolean = false;
  @Output() close = new EventEmitter<void>();

  constructor(private serverService: ServerService) {}

  request: ServerCreateRequest = {
    name: "",
    description: null,
    iconUrl: null,
  };

  createServer(): void {
    this.serverService.createServer(this.request).subscribe((res) => {
      console.log(res);
    });
  }

  onDragOver(event: DragEvent) {
    event.preventDefault();
  }

  onDrop(event: DragEvent) {
    event.preventDefault();
    if (event.dataTransfer && event.dataTransfer.files.length > 0) {
      const file = event.dataTransfer.files[0];
      console.log("Drop edilen dosya:", file);
      // Burada dosyayı işleyebilirsin
    }
  }

  onFileSelected(event: Event) {
    const input = event.target as HTMLInputElement;
    if (input.files && input.files.length > 0) {
      const file = input.files[0];
      console.log("Seçilen dosya:", file);
      // Dosyayı burada da işleyebilirsin
    }
  }

  onClose() {
    this.close.emit();
  }
}
