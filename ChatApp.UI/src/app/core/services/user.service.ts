import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import {
  BehaviorSubject,
  catchError,
  debounceTime,
  map,
  Observable,
  of,
} from 'rxjs';

import { environment } from '../../../environments/environment';
import { Channel } from '../models/channel.model';
import { User } from '../models/user.model';

@Injectable({
  providedIn: 'root',
})
export class UserService {
  private usersInCurrentChannelSubject = new BehaviorSubject<
    User[] | undefined
  >(undefined);
  public usersInCurrentChannel$ =
    this.usersInCurrentChannelSubject.asObservable();

  constructor(private httpClient: HttpClient) {}
  getUserChannels(): Observable<Channel[]> {
    return this.httpClient.get<Channel[]>(
      `${environment.apiUrl}/User/GetUserChannels`
    );
  }

  getOwnedChannels(): Observable<Channel[]> {
    return this.httpClient.get<Channel[]>(
      `${environment.apiUrl}/User/GetOwnedChannels`
    );
  }

  setCurrentChannelUsers(users: User[]): void {
    this.usersInCurrentChannelSubject.next(users);
  }
  getCurrentChannelUsers(): User[] | undefined {
    return this.usersInCurrentChannelSubject.value;
  }

  getUserByUsername(username: string): Observable<User | undefined> {
    return this.httpClient.get<User | undefined>(
      `${environment.apiUrl}/User/GetUserByUsername/?username=${username}`
    );
  }
  getUsersById(usersId: string[]): Observable<User[]> {
    return this.httpClient.post<User[]>(
      `${environment.apiUrl}/User/GetUserById`,
      usersId
    );
  }

  uploadUser(user: User): Observable<User> {
    return this.httpClient.post<User>(
      `${environment.apiUrl}/User/UpdateUser`,
      user
    );
  }

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
    formData.append('file', file);

    return this.httpClient.post<User>(
      `${environment.apiUrl}/User/UploadProfileImage`,
      formData
    );
  }
}
