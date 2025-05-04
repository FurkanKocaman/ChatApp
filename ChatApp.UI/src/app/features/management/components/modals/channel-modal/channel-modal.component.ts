import { Component, inject, Input, OnInit } from "@angular/core";
import { FormBuilder, FormsModule, ReactiveFormsModule, Validators } from "@angular/forms";
import { Channel } from "../../../../../core/models/entities";
import { CommonModule } from "@angular/common";
import { RoleService } from "../../../../../core/services/role.service";
import { RoleResponse } from "../../../../../core/models/responses/role-response.model";
import { ChannelService } from "../../../../../core/services/channel.service";
import { ChannelCreateRequest, ChannelUpdateRequest } from "../../../../../core/models/requests";

@Component({
  selector: "app-channel-modal",
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule, FormsModule],
  templateUrl: "./channel-modal.component.html",
  styleUrl: "./channel-modal.component.css",
})
export class ChannelModalComponent implements OnInit {
  @Input() channelId?: string;
  @Input() serverId!: string;

  private fb = inject(FormBuilder);
  private roleService = inject(RoleService);
  private channelService = inject(ChannelService);

  isEdit: boolean = true;

  roles: RoleResponse[] = [];
  selectedRoleId: string = "";

  selectedRoles: RoleResponse[] = [];

  form = this.fb.group({
    id: [""],
    name: ["", Validators.required],
    description: [""],
    iconUrl: [""],
    isPublic: [true, Validators.required],
    type: [0, Validators.required],
    serverId: ["", Validators.required],
    roleIds: [[""]],
  });

  ngOnInit(): void {
    if (this.serverId) {
      this.form.patchValue({
        serverId: this.serverId,
      });
    }
    if (!this.channelId) {
      this.isEdit = false;
    }
    this.getRoles();
  }

  createChannel() {
    console.log(this.toChannelCreateRequest());
    this.channelService.createChannel(this.toChannelCreateRequest()).subscribe((res) => {});
  }

  updateChannel() {
    this.channelService.updateChannel(this.toChannelUpdateRequest()).subscribe((res) => {});
  }

  getRoles() {
    if (this.serverId) {
      this.form.patchValue({
        id: this.serverId,
      });
      this.roleService.getRolesByServer(this.serverId).subscribe({
        next: (res) => {
          this.roles = res;
          this.selectedRoleId = this.roles[0].id;
          this.getSingleChannel();
        },
        error: (err) => {
          console.error(err);
        },
      });
    }
  }

  toChannelUpdateRequest(): ChannelUpdateRequest {
    const formValue = this.form.value;
    return {
      id: formValue.id!,
      name: formValue.name!,
      description: formValue.description || undefined,
      isPublic: formValue.isPublic!,
      roleIds: this.selectedRoles.map((role) => role.id),
    };
  }
  toChannelCreateRequest(): ChannelCreateRequest {
    const formValue = this.form.value;
    return {
      serverId: formValue.serverId!,
      name: formValue.name!,
      description: formValue.description || undefined,
      iconUrl: undefined,
      isPublic: formValue.isPublic!,
      roleIds: this.selectedRoles.map((role) => role.id),
      channeltype: 0,
    };
  }

  getSingleChannel() {
    if (this.channelId) {
      this.channelService.getSingleChannel(this.channelId).subscribe((res) => {
        this.form.patchValue({
          id: res.id,
          name: res.name,
          description: res.description,
          isPublic: res.isPublic,
          iconUrl: res.iconUrl,
          serverId: this.serverId,
        });
        res.roleIds.forEach((roleId) => {
          this.selectedRoles.push(this.roles.find((p) => p.id == roleId)!);
          this.updateRoleIdsInForm();
        });
      });
    }
  }

  addRole() {
    if (!this.selectedRoles.includes(this.roles.find((p) => p.id == this.selectedRoleId)!)) {
      this.selectedRoles.push(this.roles.find((p) => p.id == this.selectedRoleId)!);
      this.updateRoleIdsInForm();
    }
  }
  removeRole(role: RoleResponse) {
    this.selectedRoles = this.selectedRoles.filter((r) => r != role);
    this.updateRoleIdsInForm();
  }

  private updateRoleIdsInForm() {
    this.form.patchValue({
      roleIds: this.selectedRoles.map((r) => r.id),
    });
  }

  onSubmit() {
    if (this.isEdit) {
      this.updateChannel();
    } else {
      this.createChannel();
    }
  }
}
