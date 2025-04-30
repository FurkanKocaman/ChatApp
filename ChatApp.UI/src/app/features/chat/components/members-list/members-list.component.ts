import { CommonModule } from "@angular/common";
import { Component, EventEmitter, Input, OnInit, Output } from "@angular/core";
import { ServerMember, User } from "../../../../core/models/entities";
import { ServerService } from "../../../../core/services/server.service";

@Component({
  selector: "app-members-list",
  standalone: true,
  imports: [CommonModule],
  templateUrl: "./members-list.component.html",
  styleUrl: "./members-list.component.css",
})
export class MembersListComponent implements OnInit {
  @Input() user: User | null = null;

  @Input() isInviteMemberModalOpen: boolean = false;
  @Output() open = new EventEmitter<void>();

  isMicrophoneOpen: boolean = false;
  isHeadsetOpen: boolean = false;

  serverMembers: ServerMember[] = [];

  constructor(private serverService: ServerService) {}

  ngOnInit(): void {
    this.serverService.server$.subscribe((res) => {
      if (res) {
        this.serverService.getServerMembers(res.id).subscribe((res) => {
          if (res) {
            this.serverMembers = res;
          }
        });
      }
    });
  }

  onOpen() {
    this.open.emit();
  }

  toggleMicrophone(): void {
    this.isMicrophoneOpen = !this.isMicrophoneOpen;
  }

  toggleHeadset(): void {
    this.isHeadsetOpen = !this.isHeadsetOpen;
  }
}
