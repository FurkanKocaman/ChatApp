import { HttpClient } from "@angular/common/http";
import { Injectable } from "@angular/core";

import { BehaviorSubject, Observable, map, throwError } from "rxjs";
import { Router } from "@angular/router";
import { LoginResponse } from "../models/login-res.model";
import { LoginRequest, RegisterRequest } from "../models/requests";
import { environment } from "../../../environments/environment.development";
import { Permission } from "../models/permissions";

@Injectable({
  providedIn: "root",
})
export class AuthService {
  private permissionsSubject = new BehaviorSubject<Permission[] | null>(null);
  permissions$ = this.permissionsSubject.asObservable();

  private router: Router = new Router();
  constructor(private httpClient: HttpClient) {}

  login(credentials: LoginRequest): Observable<LoginResponse> {
    console.log("Credentials", credentials);
    return this.httpClient
      .post<{
        data: LoginResponse;
        errorMessages: string[];
        isSuccessful: boolean;
        statusCode: number;
      }>(environment.apiUrl + "auth/login", credentials)
      .pipe(
        map((response) => {
          localStorage.setItem("access_token", response.data.accessToken);
          localStorage.setItem("refresh_token", response.data.refreshToken);
          this.router.navigate(["/chat"]);
          return response.data;
        })
      );
  }

  register(credentials: RegisterRequest): Observable<LoginResponse> {
    return this.httpClient
      .post<LoginResponse>(environment.apiUrl + "auth/register", credentials)
      .pipe(
        map((response) => {
          this.router.navigate(["/chat"]);
          return response;
        })
      );
  }

  getPermissions(serverId: string): Observable<Permission[]> {
    return this.httpClient
      .get<{
        data: Permission[];
        errorMessages: string[];
        isSuccessful: boolean;
        statusCode: number;
      }>(`${environment.apiUrl}auth/permissions?serverId=${serverId}`)
      .pipe(map((p) => p.data));
  }

  setPermissionsSubject(permissions: Permission[]) {
    console.log(permissions);
    this.permissionsSubject.next(permissions);
  }

  refreshToken(): Observable<LoginResponse> {
    const refreshToken = localStorage.getItem("refreshToken");

    if (!refreshToken) {
      return throwError(() => new Error("Refresh token not found!"));
    }

    return this.httpClient
      .post<LoginResponse>(
        `${environment.apiUrl}/Auth/RefreshToken`,
        { refreshToken }, // JSON formatında gönderiyoruz
        {
          headers: { "Content-Type": "application/json" },
        }
      )
      .pipe(
        map((response) => {
          localStorage.setItem("accessToken", response.accessToken);
          localStorage.setItem("refreshToken", response.refreshToken);
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
    localStorage.removeItem("accessToken");
    localStorage.removeItem("refreshToken");
    this.router.navigate(["login"]);
  }

  isLoggedIn(): boolean {
    return localStorage.getItem("accessToken") !== null;
  }
}
