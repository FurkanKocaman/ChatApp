import { Component, inject, Input, OnInit } from "@angular/core";
import { Channel } from "../../../../../core/models/channel.model";
import { FormBuilder, FormsModule, ReactiveFormsModule, Validators } from "@angular/forms";
import { RoleDetailsResponse } from "../../../../../core/models/responses";
import { RoleService } from "../../../../../core/services/role.service";
import { RoleCreateRequest, RoleUppdateRequest } from "../../../../../core/models/requests";
import { ModalService } from "../../../../../core/services/modal.service";

@Component({
  selector: "app-role-modal",
  standalone: true,
  imports: [FormsModule, ReactiveFormsModule],
  templateUrl: "./role-modal.component.html",
  styleUrl: "./role-modal.component.css",
})
export class RoleModalComponent implements OnInit {
  @Input() serverId?: string;
  @Input() role?: RoleDetailsResponse;

  private fb = inject(FormBuilder);
  private roleService = inject(RoleService);
  private modalService = inject(ModalService);

  permissionsList = [
    "channel.create",
    "channel.edit",
    "channel.delete",
    "channel.managePermissions",
    "message.send",
    "message.edit.own",
    "message.edit.any",
    "message.delete.own",
    "message.delete.any",
    "message.pin",
    "message.viewHistory",
    "role.create",
    "role.edit",
    "role.delete",
    "role.managePermissions",
    "voice.connect",
    "voice.mute.others",
    "voice.deafen.others",
    "voice.speak",
    "voice.stream",
    "member.kick",
    "member.ban",
    "member.unban",
    "invite.create",
    "invite.delete",
    "invite.use",
  ];

  form = this.fb.group({
    id: [""],
    name: ["", Validators.required],
    level: [1],
    permissions: [[""]],
  });

  ngOnInit(): void {
    if (this.role) {
      this.form.patchValue({
        id: this.role.id,
        name: this.role.name,
        level: this.role.level,
        permissions: this.role.permissions,
      });
    }
  }

  createRole() {
    this.roleService.roleCreate(this.toRoleCreateRequest()).subscribe((res) => {
      this.onClose();
    });
  }
  updateRole() {
    this.roleService.updateRole(this.toRoleUpdateRequest()).subscribe((res) => {
      this.onClose();
    });
  }

  onPermissionChange(event: Event) {
    const checkbox = event.target as HTMLInputElement;
    const value = checkbox.value;
    const permissions = this.form.get("permissions")?.value as string[];

    if (checkbox.checked) {
      if (!permissions.includes(value)) {
        permissions.push(value);
      }
    } else {
      const index = permissions.indexOf(value);
      if (index !== -1) {
        permissions.splice(index, 1);
      }
    }

    this.form.get("permissions")?.setValue([...permissions]);
  }

  isPermissionChecked(permission: string): boolean {
    const permissions = this.form.get("permissions")?.value as string[] | null;
    return permissions?.includes(permission) ?? false;
  }
  onSubmit() {
    if (this.serverId) {
      this.createRole();
    } else {
      this.updateRole();
    }
    console.log(this.form.value);
  }

  toRoleUpdateRequest(): RoleUppdateRequest {
    const formValue = this.form.value;
    return {
      id: formValue.id!,
      name: formValue.name!,
      level: formValue.level!,
      colorHex: undefined,
      claims: formValue.permissions!,
    };
  }

  toRoleCreateRequest(): RoleCreateRequest {
    const formValue = this.form.value;
    return {
      serverId: this.serverId!,
      name: formValue.name!,
      level: formValue.level!,
      colorHex: undefined,
      claims: formValue.permissions!,
    };
  }

  onClose() {
    this.modalService.close();
  }
}
