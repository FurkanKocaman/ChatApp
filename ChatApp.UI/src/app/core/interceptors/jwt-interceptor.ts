import { HttpEvent, HttpRequest, HttpHandlerFn } from '@angular/common/http';
import { inject } from '@angular/core';
import { AuthService } from '../services/auth.service';
import { Observable } from 'rxjs';

export const JWTInterceptor = (
  req: HttpRequest<any>,
  next: HttpHandlerFn
): Observable<HttpEvent<any>> => {
  const authService = inject(AuthService);
  console.log('Http Request captured', req);

  if (localStorage.getItem('access_token')) {
    req = req.clone({
      setHeaders: {
        Authorization: `Bearer ${localStorage.getItem('access_token')}`,
      },
    });
  }

  return next(req);
};
