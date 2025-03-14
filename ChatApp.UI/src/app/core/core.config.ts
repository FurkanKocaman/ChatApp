import { provideHttpClient, withInterceptors } from '@angular/common/http';
import { ApplicationConfig } from '@angular/core';
import { JWTInterceptor } from './interceptors/jwt-interceptor';

export const coreConfig: ApplicationConfig = {
  providers: [provideHttpClient(withInterceptors([JWTInterceptor]))],
};
