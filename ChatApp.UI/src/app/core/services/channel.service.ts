import { Injectable } from "@angular/core";
import { HttpClient } from "@angular/common/http";
import { ChannelCreateRequest, ChannelUpdateRequest } from "../models/requests";
import { BehaviorSubject, map, Observable } from "rxjs";
import { environment } from "../../../environments/environment.development";

import { ChannelResponse, PaginatedResponse, SingleChannelModel } from "../models/responses";
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
      .get<ChannelResponse[]>(
        `${environment.apiUrl}servers/${id}/channels?view=summaries&page=1&pageSize=20`
      )
      .pipe(map((response) => mapChannelResponse(response)));
  }

  selectChannel(channel: Channel) {
    this.channelSubject.next(channel);
  }

  getChannelDetails(serverId: string): Observable<GetChannelDetail[]> {
    return this.httpClient
      .get<PaginatedResponse<GetChannelDetail>>(
        `${environment.apiUrl}servers/${serverId}/channels?view=details&page=1&pageSize=20`
      )
      .pipe(map((p) => p.items));
  }

  getSingleChannel(id: string): Observable<SingleChannelModel> {
    return this.httpClient
      .get<{
        data: SingleChannelModel;
        errorMessages: string[];
        isSuccessful: boolean;
        statusCode: number;
      }>(`${environment.apiUrl}channels/${id}`)
      .pipe(map((p) => p.data));
  }
}
