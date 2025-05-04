import { Component, OnInit } from "@angular/core";
import { ModeratedServerResponse } from "../../../../core/models/responses";
import { ServerService } from "../../../../core/services/server.service";

@Component({
  selector: "app-server-settings",
  standalone: true,
  imports: [],
  templateUrl: "./server-settings.component.html",
  styleUrl: "./server-settings.component.css",
})
export class ServerSettingsComponent implements OnInit {
  servers: ModeratedServerResponse[] = [];

  constructor(private serverService: ServerService) {}

  ngOnInit(): void {
    this.getModeratedServers();
  }
  getModeratedServers() {
    this.serverService.getModeratedServers().subscribe({
      next: (res) => {
        this.servers = res;
      },
      error: (err) => {
        console.error(err);
      },
    });
  }
}
