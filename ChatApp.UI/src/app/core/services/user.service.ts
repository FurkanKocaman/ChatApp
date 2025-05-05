import { HttpClient, HttpHeaders } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { BehaviorSubject, catchError, debounceTime, map, Observable, of, tap } from "rxjs";

import { environment } from "../../../environments/environment";
import { Channel } from "../models/channel.model";
import { UserResponse } from "../models/responses";
import { mapUserResponse, User } from "../models/entities/user.model";

@Injectable({
  providedIn: "root",
})
export class UserService {
  private userSubject = new BehaviorSubject<User | null>(null);
  user$ = this.userSubject.asObservable();

  constructor(private httpClient: HttpClient) {}

  getCurrentUserSnapshot(): User | null {
    return this.userSubject.value;
  }

  getCurrentUser(): Observable<User | null> {
    return this.httpClient.get<UserResponse>(`${environment.apiUrl}user/current`).pipe(
      map((response) => mapUserResponse(response)),
      tap((user) => {
        console.log(user);
        this.userSubject.next(user);
      }),
      catchError(() => of(null))
    );
  }

  // getUserChannels(): Observable<Channel[]> {
  //   return this.httpClient.get<Channel[]>(`${environment.apiUrl}/User/GetUserChannels`);
  // }

  // getOwnedChannels(): Observable<Channel[]> {
  //   return this.httpClient.get<Channel[]>(`${environment.apiUrl}/User/GetOwnedChannels`);
  // }

  // getUserByUsername(username: string): Observable<User | undefined> {
  //   return this.httpClient.get<User | undefined>(
  //     `${environment.apiUrl}/User/GetUserByUsername/?username=${username}`
  //   );
  // }
  // getUsersById(usersId: string[]): Observable<User[]> {
  //   return this.httpClient.post<User[]>(`${environment.apiUrl}/User/GetUserById`, usersId);
  // }

  // uploadUser(user: User): Observable<User> {
  //   return this.httpClient.post<User>(`${environment.apiUrl}/User/UpdateUser`, user);
  // }

  checkUsername(username: string): Observable<boolean> {
    if (!username) {
      return of(false);
    }

    return this.httpClient
      .get<{ avaliable: boolean }>(
        `${environment.apiUrl}/User/UsernameAvaliable?username=${username}`
      )
      .pipe(
        debounceTime(500),
        map((response) => response.avaliable),
        catchError(() => of(false))
      );
  }

  uploadProfilePicture(file: File, userId: string): Observable<User> {
    const formData = new FormData();
    formData.append("file", file);

    return this.httpClient.post<User>(`${environment.apiUrl}/User/UploadProfileImage`, formData);
  }

  logout() {
    this.userSubject.next(null);
  }
}
