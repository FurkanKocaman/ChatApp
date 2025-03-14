import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { BehaviorSubject, Observable } from 'rxjs';
import { Channel } from '../models/channel.model';
import { User } from '../models/user.model';
import { environment } from '../../../environments/environment.development';

@Injectable({
  providedIn: 'root',
})
export class ChannelService {
  constructor(private httpClient: HttpClient) {}

  private currentChannelSubject = new BehaviorSubject<Channel | undefined>(
    undefined
  );
  public currentChannel$ = this.currentChannelSubject.asObservable();

  setCurrentChannel(channel: Channel): void {
    this.currentChannelSubject.next(channel);
  }
  getCurrentChannel(): Channel | undefined {
    return this.currentChannelSubject.value;
  }

  getUsersByChannelId(channelId: string): Observable<User[]> {
    return this.httpClient.get<User[]>(
      `${environment.apiUrl}/Channel/GetUsersByChannelId?channelId=${channelId}`
    );
  }
  getUserCountByChannelId(channelId: string): Observable<number> {
    return this.httpClient.get<number>(
      `${environment.apiUrl}/api/ChannelGetUserCountByChannelId?channelId=${channelId}`
    );
  }
  getOnlineUserCountByChannelId(channelId: string): Observable<number> {
    return this.httpClient.get<number>(
      `${environment.apiUrl}/api/GetOnlineUserCountByChannelId?channelId=${channelId}`
    );
  }
}
