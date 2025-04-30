import { Component, EventEmitter, Input, Output } from "@angular/core";
import { GenerateInvitationToken } from "../../../../core/models/requests/generate-invitation-token.model";
import { ServerService } from "../../../../core/services/server.service";
import { TokenService } from "../../../../core/services/token.service";
import { FormsModule } from "@angular/forms";
import { Clipboard } from "@angular/cdk/clipboard";
import { ToastService } from "../../../../core/services/toast.service";

@Component({
  selector: "app-invite-member-modal",
  standalone: true,
  imports: [FormsModule],
  templateUrl: "./invite-member-modal.component.html",
  styleUrl: "./invite-member-modal.component.css",
})
export class InviteMemberModalComponent {
  @Input() isInviteMemberModalOpen: boolean = false;
  @Output() close = new EventEmitter<void>();

  token: string = "";

  constructor(
    private serverService: ServerService,
    private tokenService: TokenService,
    private clipBoard: Clipboard,
    private toastService: ToastService
  ) {}

  request: GenerateInvitationToken = {
    count: 1,
    serverId: "",
  };

  generateLink() {
    const server = this.serverService.getCurrentServerSnapshot();
    console.log(server);
    if (server) {
      this.request.serverId = server?.id;

      this.tokenService.generateToken(this.request).subscribe({
        next: (res) => {
          console.log(res);
          this.token = res;
        },
      });
    }
  }

  copyToClipboard() {
    this.clipBoard.copy(this.token);
    this.toastService.showToast("Copied to clipboard", "success", 5000);
  }

  onClose() {
    this.close.emit();
  }
}
