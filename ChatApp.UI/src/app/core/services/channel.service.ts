import { Injectable } from "@angular/core";
import { HttpClient } from "@angular/common/http";
import { ChannelCreateRequest, ChannelUpdateRequest } from "../models/requests";
import { BehaviorSubject, map, Observable } from "rxjs";
import { environment } from "../../../environments/environment.development";

import { ChannelResponse, SingleChannelModel } from "../models/responses";
import { Channel, mapChannelResponse } from "../models/entities";
import { GetChannelDetail } from "../models/responses/get-channel-detail.model";

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
  updateChannel(request: ChannelUpdateRequest): Observable<string> {
    return this.httpClient
      .put<{
        data: string;
        errorMessages: string[];
        isSuccessful: boolean;
        statusCode: number;
      }>(`${environment.apiUrl}channels/update`, request)
      .pipe(map((res) => res.data));
  }

  getChannelsByServerId(id: string): Observable<Channel[]> {
    return this.httpClient
      .get<{ value: ChannelResponse[] }>(`${environment.apiUrl}odata/server-channels/${id}`)
      .pipe(map((response) => mapChannelResponse(response.value)));
  }

  selectChannel(channel: Channel) {
    this.channelSubject.next(channel);
  }

  getChannelDetails(serverId: string): Observable<GetChannelDetail[]> {
    return this.httpClient
      .get<{ value: GetChannelDetail[] }>(`${environment.apiUrl}odata/channel-details/${serverId}`)
      .pipe(map((p) => p.value));
  }

  getSingleChannel(id: string): Observable<SingleChannelModel> {
    return this.httpClient
      .get<{
        data: SingleChannelModel;
        errorMessages: string[];
        isSuccessful: boolean;
        statusCode: number;
      }>(`${environment.apiUrl}channels/get?id=${id}`)
      .pipe(map((p) => p.data));
  }
}
