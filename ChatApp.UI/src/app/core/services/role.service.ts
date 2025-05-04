import { HttpClient } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { map, Observable } from "rxjs";

import { environment } from "../../../environments/environment.development";
import { RoleCreateRequest, RoleUppdateRequest } from "../models/requests";
import { RoleDetailsResponse, RoleResponse } from "../models/responses";

@Injectable({
  providedIn: "root",
})
export class RoleService {
  constructor(private httpClient: HttpClient) {}

  roleCreate(request: RoleCreateRequest): Observable<string> {
    return this.httpClient
      .post<{
        data: string;
        errorMessages: string[];
        isSuccessful: boolean;
        statusCode: number;
      }>(`${environment.apiUrl}roles/create`, request)
      .pipe(map((p) => p.data));
  }

  updateRole(request: RoleUppdateRequest): Observable<string> {
    return this.httpClient
      .put<{
        data: string;
        errorMessages: string[];
        isSuccessful: boolean;
        statusCode: number;
      }>(`${environment.apiUrl}roles/update`, request)
      .pipe(map((p) => p.data));
  }

  getRolesByServer(serverId: string): Observable<RoleResponse[]> {
    return this.httpClient.get<RoleResponse[]>(
      `${environment.apiUrl}roles/get?serverId=${serverId}`
    );
  }

  getRoleDetailsByServer(serverId: string): Observable<RoleDetailsResponse[]> {
    return this.httpClient
      .get<{ value: RoleDetailsResponse[] }>(`${environment.apiUrl}odata/roles/${serverId}`)
      .pipe(map((p) => p.value));
  }
}
