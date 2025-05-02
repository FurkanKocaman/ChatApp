import { Component, OnInit } from "@angular/core";
import { FormsModule } from "@angular/forms";
import { CommonModule } from "@angular/common";
import { ServerListComponent } from "../../components/server-list/server-list.component";
import { ChannelListComponent } from "../../components/channel-list/channel-list.component";
import { ChatAreaComponent } from "../../components/chat-area/chat-area.component";
import { MembersListComponent } from "../../components/members-list/members-list.component";
import { User } from "../../../../core/models/entities";
import { UserService } from "../../../../core/services/user.service";
import { ServerCreateModalComponent } from "../../components/server-create-modal/server-create-modal.component";
import { ChannelCreateModalComponent } from "../../components/channel-create-modal/channel-create-modal.component";
import { SignalChatService } from "../../../../core/services/signal-chat.service";
import { InviteMemberModalComponent } from "../../components/invite-member-modal/invite-member-modal.component";

@Component({
  selector: "app-chat-layout",
  standalone: true,
  imports: [
    FormsModule,
    CommonModule,
    ServerListComponent,
    ChannelListComponent,
    ChatAreaComponent,
    MembersListComponent,
    ServerCreateModalComponent,
    ChannelCreateModalComponent,
    InviteMemberModalComponent,
  ],
  templateUrl: "./chat-layout.component.html",
  styleUrl: "./chat-layout.component.css",
})
export class ChatLayoutComponent implements OnInit {
  user: User | null = null;
  isCreateServerModalOpen: boolean = false;
  isCreateChannelModalOpen: boolean = false;
  isInviteMemberModalOpen: boolean = false;

  isSidebarOpen: boolean = false;

  constructor(private userService: UserService, private signalService: SignalChatService) {}

  ngOnInit(): void {
    this.userService.getCurrentUser().subscribe((user) => {
      this.user = user;
      this.signalService.startConnection();
    });
  }

  openCreateServerModal() {
    this.isCreateServerModalOpen = true;
  }
  closeCreateServerModal() {
    this.isCreateServerModalOpen = false;
  }

  openCreateChannelModal() {
    this.isCreateChannelModalOpen = true;
  }
  closeCreateChannelModal() {
    this.isCreateChannelModalOpen = false;
  }

  openInviteMemberModal() {
    this.isInviteMemberModalOpen = true;
  }
  closeInviteMemberModal() {
    this.isInviteMemberModalOpen = false;
  }

  openSidebar() {
    this.isSidebarOpen = true;
  }

  closeSidebar() {
    this.isSidebarOpen = false;
    console.log(this.isSidebarOpen);
  }
}
