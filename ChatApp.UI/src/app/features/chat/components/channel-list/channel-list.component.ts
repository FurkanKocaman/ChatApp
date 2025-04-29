import { CommonModule } from "@angular/common";
import { Component, EventEmitter, Input, OnInit, Output } from "@angular/core";
import { ServerService } from "../../../../core/services/server.service";
import { Server } from "../../../../core/models/entities";
import { Channel } from "../../../../core/models/entities/channel.model";
import { ChannelService } from "../../../../core/services/channel.service";
import { SignalChatService } from "../../../../core/services/signal-chat.service";

@Component({
  selector: "app-channel-list",
  standalone: true,
  imports: [CommonModule],
  templateUrl: "./channel-list.component.html",
  styleUrl: "./channel-list.component.css",
})
export class ChannelListComponent implements OnInit {
  isOpen = false;
  @Input() isCreateChannelModalOpen: boolean = false;
  @Output() open = new EventEmitter<void>();

  selectedServer: Server | null = null;
  channels: Channel[] = [];
  selectedChannelId: string = "";

  constructor(
    private serverService: ServerService,
    private channelService: ChannelService,
    private signalService: SignalChatService
  ) {}

  ngOnInit(): void {
    this.serverService.server$.subscribe((server) => {
      this.selectedServer = server;

      if (server) {
        this.channelService.getChannelsByServerId(server!.id).subscribe((res) => {
          this.channels = res;
          console.log(res);
        });
      }
    });
  }

  onOpen() {
    this.toggleMenu();
    this.open.emit();
  }

  selectChannel(channel: Channel) {
    this.channelService.selectChannel(channel);
    this.joinChannel(channel.id);
  }

  joinChannel(id: string) {
    if (this.selectedChannelId != "") this.leaveChannel(this.selectedChannelId);
    this.signalService.joinChannel(id);
    this.selectedChannelId = id;
  }
  leaveChannel(id: string) {
    this.signalService.leaveChannel(id);
  }

  toggleMenu() {
    this.isOpen = !this.isOpen;
  }
}
