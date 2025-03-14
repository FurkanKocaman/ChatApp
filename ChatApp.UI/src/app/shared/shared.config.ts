import { CommonModule } from '@angular/common';
import { ApplicationConfig, importProvidersFrom } from '@angular/core';

export const sharedConfig: ApplicationConfig = {
  providers: [importProvidersFrom(CommonModule)],
};
