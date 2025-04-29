import { HttpClient } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { MessageSendRequest } from "../models/requests";
import { map, Observable } from "rxjs";
import { environment } from "../../../environments/environment.development";
import { MessageGetAllResponse } from "../models/responses";
import { mapMessageResponse, Message } from "../models/entities";

@Injectable({
  providedIn: "root",
})
export class MessageService {
  constructor(private httpClient: HttpClient) {}

  sendMessage(request: MessageSendRequest): Observable<string> {
    return this.httpClient
      .post<{
        data: string;
        errorMessages: string[];
        isSuccessful: boolean;
        statusCode: number;
      }>(`${environment.apiUrl}messages/create`, request)
      .pipe(
        map((res) => {
          return res.data;
        })
      );
  }

  getMessages(id: string): Observable<Message[]> {
    return this.httpClient
      .get<{ value: MessageGetAllResponse[] }>(`${environment.apiUrl}odata/messages/${id}`)
      .pipe(map((response) => mapMessageResponse(response.value)));
  }
}
