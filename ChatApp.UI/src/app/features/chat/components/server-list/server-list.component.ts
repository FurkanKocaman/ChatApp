import { AfterViewInit, Component, EventEmitter, Input, OnInit, Output } from "@angular/core";
import { Server, ServerMember } from "../../../../core/models/entities";
import { ServerService } from "../../../../core/services/server.service";
import { CommonModule, UpperCasePipe } from "@angular/common";
import { SignalChatService } from "../../../../core/services/signal-chat.service";

@Component({
  selector: "app-server-list",
  standalone: true,
  imports: [UpperCasePipe, CommonModule],
  templateUrl: "./server-list.component.html",
  styleUrl: "./server-list.component.css",
})
export class ServerListComponent implements OnInit, AfterViewInit {
  @Input() isServerCreateModalOpen: boolean = false;
  @Input() sidebarOpen: boolean = false;

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

  ngAfterViewInit(): void {
    const messageContainers = document.querySelectorAll(".server-list");
    messageContainers.forEach((el) => {
      el.classList.add("notranslate");
      el.setAttribute("translate", "no");
    });
  }

  setSelectedServer(server: Server) {
    this.serverService.setSelectedServer(server);
  }

  onOpen() {
    this.open.emit();
  }
}
