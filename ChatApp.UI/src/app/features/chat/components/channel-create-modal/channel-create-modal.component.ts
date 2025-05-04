import { CommonModule } from "@angular/common";
import { Component, EventEmitter, Input, OnInit, Output } from "@angular/core";
import { FormsModule } from "@angular/forms";
import { ChannelCreateRequest } from "../../../../core/models/requests";
import { ChannelService } from "../../../../core/services/channel.service";
import { ServerService } from "../../../../core/services/server.service";

@Component({
  selector: "app-channel-create-modal",
  standalone: true,
  imports: [FormsModule, CommonModule],
  templateUrl: "./channel-create-modal.component.html",
  styleUrl: "./channel-create-modal.component.css",
})
export class ChannelCreateModalComponent implements OnInit {
  @Input() isCreateChannelModalOpen: boolean = false;
  @Output() close = new EventEmitter<void>();

  isPublic: boolean = true;
  roles: string[] = ["manager", "visitor", "king", "pleb"];
  filteredRoles: string[] = [];
  selectedRoles: string[] = [];

  isRoleListOpen: boolean = false;

  request: ChannelCreateRequest = {
    serverId: "",
    name: "",
    description: undefined,
    iconUrl: undefined,
    channeltype: 0,
    isPublic: false,
    roleIds: [],
  };

  ngOnInit(): void {
    this.serverService.server$.subscribe((res) => {
      if (res) this.request.serverId = res?.id;
    });
  }

  constructor(private channelService: ChannelService, private serverService: ServerService) {}

  createChannel() {
    console.log(this.request);
    this.channelService.createChannel(this.request).subscribe({
      next: (res) => {
        console.log(res);
      },
      error: (err) => {
        console.log(err);
      },
      complete: () => {
        this.onClose();
      },
    });
  }

  filterRoles(filter: string) {
    this.filteredRoles = this.roles;
    this.filteredRoles = this.roles.filter(
      (role) =>
        role.toLowerCase().includes(filter.toLowerCase()) && !this.selectedRoles.includes(role)
    );
  }

  selectRole(role: string) {
    if (!this.selectedRoles.includes(role)) {
      this.selectedRoles.push(role);
      this.filteredRoles = [];
      this.isRoleListOpen = false;
    }

    const input = document.getElementById("role_name") as HTMLInputElement;
    if (input) {
      input.value = "";
    }
  }
  removeRole(role: string) {
    if (this.selectedRoles.includes(role)) {
      this.selectedRoles = this.selectedRoles.filter((p) => p != role);
    }
  }

  onBlur() {
    setTimeout(() => {
      this.isRoleListOpen = false;
    }, 100);
  }
  onClose() {
    this.close.emit();
  }
}
