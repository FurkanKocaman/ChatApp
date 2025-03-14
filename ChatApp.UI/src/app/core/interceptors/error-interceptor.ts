import {
  HttpErrorResponse,
  HttpEvent,
  HttpHandlerFn,
  HttpRequest,
} from '@angular/common/http';
import { inject } from '@angular/core';
import { catchError, Observable, switchMap, throwError } from 'rxjs';
import { AuthService } from '../services/auth.service';

export const ErrorInterceptor = (
  req: HttpRequest<any>,
  next: HttpHandlerFn
): Observable<HttpEvent<any>> => {
  const authService = inject(AuthService);

  return next(req).pipe(
    catchError((error) => {
      console.log('ERROR', error);
      if (error instanceof HttpErrorResponse && error.status === 401) {
        console.error('401 Error');
        if (!req.url.includes('/login')) {
          authService.refreshToken().subscribe((response) => {
            localStorage.setItem('accessToken', response.accessToken);
            localStorage.setItem('refreshToken', response.refreshToken);
          });
          return throwError(() => error);
          // return handle401Error(req, next, authService);
        } else {
          return throwError(() => error);
        }
      } else {
        return throwError(() => error);
      }
    })
  );

  const addToken = (req: HttpRequest<any>): HttpRequest<any> => {
    const accessToken = localStorage.getItem('accessToken');

    if (accessToken) {
      return req.clone({
        setHeaders: {
          Authorization: `Bearer ${accessToken}`,
        },
      });
    }
    return req;
  };

  const handle401Error = (
    req: HttpRequest<any>,
    next: HttpHandlerFn,
    authService: AuthService
  ): Observable<HttpEvent<any>> => {
    return authService.refreshToken().pipe(
      switchMap(() => {
        return next(addToken(req));
      }),
      catchError((error) => {
        console.error('Failed refresh token:', error);
        authService.logout();
        return throwError(() => error);
      })
    );
  };
};
