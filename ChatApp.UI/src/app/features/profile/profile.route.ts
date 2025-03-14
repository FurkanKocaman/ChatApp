import { Routes } from '@angular/router';
import { authGuard } from '../../core/guards/auth.guard';

export const PROFILE_ROUTES: Routes = [
  {
    path: '',
    loadComponent: () =>
      import('../../layouts/profile-layout/profile-layout.component').then(
        (m) => m.ProfileLayoutComponent
      ),
    children: [
      {
        path: 'overview/:id',
        loadComponent: () =>
          import('./profile-overview/profile-overview.component').then(
            (m) => m.ProfileOverviewComponent
          ),
      },
      {
        path: 'settings',
        loadComponent: () =>
          import('./profile-settings/profile-settings.component').then(
            (m) => m.ProfileSettingsComponent
          ),
        canActivate: [authGuard],
      },
      {
        path: 'channels',
        loadComponent: () =>
          import('./profile-channels/profile-channels.component').then(
            (m) => m.ProfileChannelsComponent
          ),
        canActivate: [authGuard],
      },
    ],
  },
];
