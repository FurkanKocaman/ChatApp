import { ApplicationConfig, provideZoneChangeDetection } from '@angular/core';
import { provideRouter } from '@angular/router';
import { routes } from './app.routes';
import { coreConfig } from './core/core.config';
import { sharedConfig } from './shared/shared.config';
import { provideAnimationsAsync } from '@angular/platform-browser/animations/async';

export const appConfig: ApplicationConfig = {
  providers: [
    provideZoneChangeDetection({ eventCoalescing: true }),
    provideRouter(routes),
    ...coreConfig.providers,
    ...sharedConfig.providers, provideAnimationsAsync(),
  ],
};
