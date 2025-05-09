import { Routes } from "@angular/router";
import { authGuard } from "./core/guards/auth.guard";

export const routes: Routes = [
  { path: "", redirectTo: "chat", pathMatch: "full" },
  {
    path: "auth",
    loadChildren: () => import("./features/auth/auth.route").then((m) => m.AUTH_ROUTES),
  },
  {
    path: "chat",
    loadComponent: () =>
      import("./features/chat/layouts/chat-layout/chat-layout.component").then(
        (m) => m.ChatLayoutComponent
      ),
    canActivate: [authGuard],
  },
  {
    path: "join-server",
    loadComponent: () =>
      import("./features/join-server/join-server.component").then((m) => m.JoinServerComponent),
    canActivate: [authGuard],
  },
  {
    path: "management",
    loadComponent: () =>
      import("./features/management/layouts/management-layout/management-layout.component").then(
        (m) => m.ManagementLayoutComponent
      ),
    loadChildren: () =>
      import("./features/management/management.route").then((m) => m.MANAGEMENT_ROUTES),
    // canActivate: [authGuard],
  },
  // { path: '', component: ChatPageComponent, canActivate: [authGuard] },
  // {
  //   path: 'chat',
  //   component: ChatPageComponent,
  //   canActivate: [authGuard],
  // },
  // { path: 'login', component: LoginPageComponent },
  // {
  //   path: 'register',
  //   component: RegisterPageComponent,
  // },
  // {
  //   path: 'profile',
  //   loadComponent: () =>
  //     import('./pages/profile-page/profile-page.component').then(
  //       (c) => c.ProfilePageComponent
  //     ),
  //   children: [
  //     {
  //       path: 'info/:id',
  //       loadComponent: () =>
  //         import(
  //           './profile-components/profile-information/profile-information.component'
  //         ).then((c) => c.ProfileInformationComponent),
  //     },
  //     {
  //       path: 'settings/:id',
  //       loadComponent: () =>
  //         import(
  //           './profile-components/profile-settings/profile-settings.component'
  //         ).then((c) => c.ProfileSettingsComponent),
  //       canActivate: [authGuard],
  //     },
  //     {
  //       path: 'chats/:id',
  //       loadComponent: () =>
  //         import(
  //           './profile-components/profile-chats/profile-chats.component'
  //         ).then((c) => c.ProfileChatsComponent),
  //       canActivate: [authGuard],
  //     },
  //     { path: '', redirectTo: 'info', pathMatch: 'full' },
  //   ],
  // },
  // { path: 'admin', component: AdminPageComponent, canActivate: [adminGuard] },
];
