import { Routes } from '@angular/router';
import { authGuard } from './core/guards/auth.guard';

export const routes: Routes = [
  { path: '', redirectTo: 'chat', pathMatch: 'full' },
  {
    path: 'auth',
    loadChildren: () =>
      import('./features/auth/auth.route').then((m) => m.AUTH_ROUTES),
  },
  {
    path: 'chat',
    loadComponent: () =>
      import('./layouts/main-layout/main-layout.component').then(
        (m) => m.MainLayoutComponent
      ),
    canActivate: [authGuard],
  },
  {
    path: 'profile',
    loadChildren: () =>
      import('./features/profile/profile.route').then((m) => m.PROFILE_ROUTES),
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
