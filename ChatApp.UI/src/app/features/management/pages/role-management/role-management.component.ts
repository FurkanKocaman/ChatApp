import { Component, ElementRef, HostListener, OnInit } from "@angular/core";
import { ModalService } from "../../../../core/services/modal.service";
import { RoleModalComponent } from "../../components/modals/role-modal/role-modal.component";
import { ServerService } from "../../../../core/services/server.service";
import { Server } from "../../../../core/models/entities";
import { RoleDetailsResponse } from "../../../../core/models/responses";
import { RoleService } from "../../../../core/services/role.service";

@Component({
  selector: "app-role-management",
  standalone: true,
  imports: [],
  templateUrl: "./role-management.component.html",
  styleUrl: "./role-management.component.css",
})
export class RoleManagementComponent implements OnInit {
  isServerMenuOpen: boolean = false;

  selectedServer: Server | null = null;
  servers: Server[] = [];

  roles: RoleDetailsResponse[] = [];

  toggleServerMenu() {
    this.isServerMenuOpen = !this.isServerMenuOpen;
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

  openRoleCreateModal() {
    this.modalService.open(RoleModalComponent, { serverId: this.selectedServer?.id });
  }
  openRoleEditModal(role: RoleDetailsResponse) {
    this.modalService.open(RoleModalComponent, { role: role });
  }

  constructor(
    private elementRef: ElementRef,
    private modalService: ModalService,
    private serverService: ServerService,
    private roleService: RoleService
  ) {}
  ngOnInit(): void {
    this.getUserJoinedServers();
    // this.openRoleCreateModal();
  }

  getUserJoinedServers() {
    this.serverService.getUserJoinedServers().subscribe({
      next: (res) => {
        if (res) {
          this.setSelectedServer(res[0]);
        }
        this.servers = res;
      },
      error: (err) => {
        console.error(err);
      },
    });
  }

  getRolesByServer() {
    if (this.selectedServer) {
      this.roleService.getRoleDetailsByServer(this.selectedServer.id).subscribe((res) => {
        if (res) {
          this.roles = res;
        }
      });
    }
  }

  setSelectedServer(server: Server) {
    this.selectedServer = server;
    this.serverService.setSelectedServer(server);
    this.getRolesByServer();
  }
}
