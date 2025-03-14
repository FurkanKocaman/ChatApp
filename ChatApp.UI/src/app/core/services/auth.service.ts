import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';

import { BehaviorSubject, Observable, map, throwError } from 'rxjs';
import { Router } from '@angular/router';
import { environment } from '../../../environments/environment';
import { LoginRequest } from '../models/login-req.model';
import { User } from '../models/user.model';
import { LoginResponse } from '../models/login-res.model';
import { RegisterRequest } from '../models/register-req.model';

@Injectable({
  providedIn: 'root',
})
export class AuthService {
  private currentUserSubject = new BehaviorSubject<User | any>(null);
  public currentUser$ = this.currentUserSubject.asObservable();

  private router: Router = new Router();
  constructor(private httpClient: HttpClient) {}

  login(credentials: LoginRequest): Observable<LoginResponse> {
    return this.httpClient
      .post<LoginResponse>(environment.apiUrl + '/Auth/Login', credentials)
      .pipe(
        map((response) => {
          localStorage.setItem('access_token', response.accessToken);
          localStorage.setItem('refresh_token', response.refreshToken);
          return response;
        })
      );
  }

  getUserData() {
    return this.httpClient
      .get(`${environment.apiUrl}/Auth/GetUserByToken`)
      .subscribe({
        next: (res) => {
          this.currentUserSubject.next(res);
          this.router.navigate(['/chat']);
        },
        error: (err) => {
          console.error(err);
        },
      });
  }

  getCurrentUser(): User {
    return this.currentUserSubject.value;
  }

  register(credentials: RegisterRequest): Observable<LoginResponse> {
    return this.httpClient
      .post<LoginResponse>(environment.apiUrl + '/Auth/Register', credentials)
      .pipe(
        map((response) => {
          this.router.navigate(['/chat']);
          return response;
        })
      );
  }

  refreshToken(): Observable<LoginResponse> {
    const refreshToken = localStorage.getItem('refreshToken');

    if (!refreshToken) {
      return throwError(() => new Error('Refresh token not found!'));
    }

    return this.httpClient
      .post<LoginResponse>(
        `${environment.apiUrl}/Auth/RefreshToken`,
        { refreshToken }, // JSON formatında gönderiyoruz
        {
          headers: { 'Content-Type': 'application/json' },
        }
      )
      .pipe(
        map((response) => {
          localStorage.setItem('accessToken', response.accessToken);
          localStorage.setItem('refreshToken', response.refreshToken);
          return response;
        })
      );
  }

  // private getRefreshTokenFromCookie(): string | null {
  //   const cookieString = document.cookie;
  //   const cookieArray = cookieString.split(';');

  //   for (const cookie in cookieArray) {
  //     const [name, value] = cookie.split('=');

  //     if (name == 'refreshToken') {
  //       return value;
  //     }
  //   }
  //   return null;
  // }

  logout() {
    localStorage.removeItem('accessToken');
    localStorage.removeItem('refreshToken');
    this.router.navigate(['login']);
  }

  isLoggedIn(): boolean {
    return localStorage.getItem('accessToken') !== null;
  }
}
