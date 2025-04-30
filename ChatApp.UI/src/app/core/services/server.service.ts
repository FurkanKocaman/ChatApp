import { HttpClient } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { ServerCreateRequest } from "../models/requests";
import { BehaviorSubject, map, Observable } from "rxjs";
import { environment } from "../../../environments/environment.development";
import { GetUserJoinedServersQueryResponse, ServerMemberGetAllResponse } from "../models/responses";
import {
  mapServerMemberResponse,
  mapServerResponse,
  Server,
  ServerMember,
} from "../models/entities";
import { AuthService } from "./auth.service";

@Injectable({
  providedIn: "root",
})
export class ServerService {
  private serverSubject = new BehaviorSubject<Server | null>(null);
  server$ = this.serverSubject.asObservable();

  constructor(private httpClient: HttpClient, private authService: AuthService) {}

  createServer(request: ServerCreateRequest): Observable<string> {
    return this.httpClient
      .post<{
        data: string;
        errorMessages: string[];
        isSuccessful: boolean;
        statusCode: number;
      }>(`${environment.apiUrl}servers/create`, request)
      .pipe(
        map((res) => {
          return res.data;
        })
      );
  }
  getUserJoinedServers(): Observable<Server[]> {
    return this.httpClient
      .get<{ value: GetUserJoinedServersQueryResponse[] }>(
        `${environment.apiUrl}odata/user-servers`
      )
      .pipe(map((response) => mapServerResponse(response.value)));
  }

  async setSelectedServer(server: Server) {
    this.serverSubject.next(server);

    this.authService.getPermissions(server.id).subscribe((res) => {
      this.authService.setPermissionsSubject(res);
    });
  }

  getCurrentServerSnapshot(): Server | null {
    return this.serverSubject.value;
  }

  getServerMembers(id: string): Observable<ServerMember[]> {
    return this.httpClient
      .get<{ value: ServerMemberGetAllResponse[] }>(
        `${environment.apiUrl}odata/server-members/${id}`
      )
      .pipe(map((res) => mapServerMemberResponse(res.value)));
  }

  joinServerByToken(token: string): Observable<string> {
    return this.httpClient
      .post<{
        data: string;
        errorMessages: string[];
        isSuccessful: boolean;
        statusCode: number;
      }>(`${environment.apiUrl}server-members/join-by-token`, { token: token })
      .pipe(map((p) => p.data));
  }
}
