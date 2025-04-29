import { Component, EventEmitter, Input, OnInit, Output } from "@angular/core";
import { Server, ServerMember } from "../../../../core/models/entities";
import { ServerService } from "../../../../core/services/server.service";
import { UpperCasePipe } from "@angular/common";
import { SignalChatService } from "../../../../core/services/signal-chat.service";

@Component({
  selector: "app-server-list",
  standalone: true,
  imports: [UpperCasePipe],
  templateUrl: "./server-list.component.html",
  styleUrl: "./server-list.component.css",
})
export class ServerListComponent implements OnInit {
  @Input() isServerCreateModalOpen: boolean = false;
  @Output() open = new EventEmitter<void>();

  constructor(private serverService: ServerService, private signalService: SignalChatService) {}

  servers: Server[] = [];
  serverMembers: ServerMember[] = [];

  ngOnInit(): void {
    this.serverService.getUserJoinedServers().subscribe((res) => {
      this.servers = res;
    });
    this.signalService.currentSignal$.subscribe((res) => {
      if (res) {
        this.signalService.JoinServers();
      }
    });
  }

  setSelectedServer(server: Server) {
    this.serverService.setSelectedServer(server);
  }

  onOpen() {
    this.open.emit();
  }
}
