import { Component, ElementRef, HostListener, OnInit } from "@angular/core";
import { ModalService } from "../../../../core/services/modal.service";
import { ChannelModalComponent } from "../../components/modals/channel-modal/channel-modal.component";
import { ServerService } from "../../../../core/services/server.service";
import { Server } from "../../../../core/models/entities";
import { ChannelService } from "../../../../core/services/channel.service";
import { GetChannelDetail } from "../../../../core/models/responses/get-channel-detail.model";

@Component({
  selector: "app-channel-management",
  standalone: true,
  imports: [],
  templateUrl: "./channel-management.component.html",
  styleUrl: "./channel-management.component.css",
})
export class ChannelManagementComponent implements OnInit {
  isServerMenuOpen: boolean = false;

  selectedServer: Server | null = null;
  servers: Server[] = [];

  channels: GetChannelDetail[] = [];

  toggleServerMenu() {
    this.isServerMenuOpen = !this.isServerMenuOpen;
  }

  ngOnInit(): void {
    this.getUserJoinedServers();
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
    this.modalService.open(ChannelModalComponent, { serverId: this.selectedServer?.id });
  }

  openChannelEditModal(channelId: string) {
    this.modalService.open(ChannelModalComponent, {
      serverId: this.selectedServer?.id,
      channelId: channelId,
    });
  }

  getUserJoinedServers() {
    this.serverService.getUserJoinedServers().subscribe({
      next: (res) => {
        if (res) {
          this.setSelectedServer(res[0]);
          this.getChannels();
        }
        this.servers = res;
      },
      error: (err) => {
        console.error(err);
      },
    });
  }

  setSelectedServer(server: Server) {
    this.selectedServer = server;
    this.getChannels();
  }

  getChannels() {
    if (this.selectedServer) {
      this.channelService.getChannelDetails(this.selectedServer.id).subscribe({
        next: (res) => {
          this.channels = res;
        },
        error: (err) => {
          console.error(err);
        },
      });
    }
  }

  constructor(
    private elementRef: ElementRef,
    private modalService: ModalService,
    private serverService: ServerService,
    private channelService: ChannelService
  ) {}
}
