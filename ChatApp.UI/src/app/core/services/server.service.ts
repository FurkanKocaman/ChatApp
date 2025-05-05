import { HttpClient } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { ServerCreateRequest } from "../models/requests";
import { BehaviorSubject, map, Observable } from "rxjs";
import { environment } from "../../../environments/environment.development";
import {
  GetUserJoinedServersQueryResponse,
  ModeratedServerResponse,
  PaginatedResponse,
  ServerMemberGetAllResponse,
} from "../models/responses";
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
      .get<GetUserJoinedServersQueryResponse[]>(`${environment.apiUrl}servers`)
      .pipe(map((response) => mapServerResponse(response)));
  }

  getModeratedServers(): Observable<ModeratedServerResponse[]> {
    return this.httpClient
      .get<ModeratedServerResponse[]>(`${environment.apiUrl}servers/moderated`)
      .pipe(map((response) => response));
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
      .get<PaginatedResponse<ServerMemberGetAllResponse>>(
        `${environment.apiUrl}servers/${id}/members?view=details&page=1&pageSize=20`
      )
      .pipe(map((res) => mapServerMemberResponse(res.items)));
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
