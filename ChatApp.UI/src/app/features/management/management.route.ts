import { Routes } from "@angular/router";

export const MANAGEMENT_ROUTES: Routes = [
  {
    path: "server-management",
    loadComponent: () =>
      import("./pages/server-settings/server-settings.component").then(
        (m) => m.ServerSettingsComponent
      ),
  },
  {
    path: "channel-management",
    loadComponent: () =>
      import("./pages/channel-management/channel-management.component").then(
        (m) => m.ChannelManagementComponent
      ),
  },
  {
    path: "role-management",
    loadComponent: () =>
      import("./pages/role-management/role-management.component").then(
        (m) => m.RoleManagementComponent
      ),
  },
  {
    path: "member-management",
    loadComponent: () =>
      import("./pages/member-management/member-management.component").then(
        (m) => m.MemberManagementComponent
      ),
  },
  {
    path: "moderation",
    loadComponent: () =>
      import("./pages/moderation/moderation.component").then((m) => m.ModerationComponent),
  },
];
