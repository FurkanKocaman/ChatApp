import { Injectable } from "@angular/core";
import { HttpClient } from "@angular/common/http";
import { ChannelCreateRequest } from "../models/requests";
import { BehaviorSubject, map, Observable } from "rxjs";
import { environment } from "../../../environments/environment.development";

import { ChannelResponse } from "../models/responses";
import { Channel, mapChannelResponse } from "../models/entities";

@Injectable({
  providedIn: "root",
})
export class ChannelService {
  private channelSubject = new BehaviorSubject<Channel | null>(null);
  channel$ = this.channelSubject.asObservable();
  constructor(private httpClient: HttpClient) {}

  createChannel(request: ChannelCreateRequest): Observable<string> {
    return this.httpClient
      .post<{
        data: string;
        errorMessages: string[];
        isSuccessful: boolean;
        statusCode: number;
      }>(`${environment.apiUrl}channels/create`, request)
      .pipe(map((res) => res.data));
  }

  getChannelsByServerId(id: string): Observable<Channel[]> {
    return this.httpClient
      .get<ChannelResponse[]>(`${environment.apiUrl}channels/get?ServerId=${id}`)
      .pipe(map((response) => mapChannelResponse(response)));
  }

  selectChannel(channel: Channel) {
    this.channelSubject.next(channel);
  }
}
