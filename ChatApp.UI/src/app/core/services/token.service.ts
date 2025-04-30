import { Injectable } from "@angular/core";
import { GenerateInvitationToken } from "../models/requests/generate-invitation-token.model";
import { map, Observable } from "rxjs";
import { HttpClient } from "@angular/common/http";
import { environment } from "../../../environments/environment.development";
import { InviteValidationResponse } from "../models/responses/invite-validation-response.model";

@Injectable({
  providedIn: "root",
})
export class TokenService {
  constructor(private httpClient: HttpClient) {}

  generateToken(request: GenerateInvitationToken): Observable<string> {
    return this.httpClient
      .post<{
        data: string;
        errorMessages: string[];
        isSuccessful: boolean;
        statusCode: number;
      }>(`${environment.apiUrl}token/create`, request)
      .pipe(map((p) => p.data));
  }

  validateToken(token: string): Observable<InviteValidationResponse> {
    return this.httpClient
      .get<{
        data: InviteValidationResponse;
        errorMessages: string[];
        isSuccessful: boolean;
        statusCode: number;
      }>(`${environment.apiUrl}token/validate?token=${token}`)
      .pipe(map((p) => p.data));
  }
}
