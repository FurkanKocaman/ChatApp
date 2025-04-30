import { Component, OnInit } from "@angular/core";
import { ActivatedRoute, Router } from "@angular/router";
import { InviteValidationResponse } from "../../core/models/responses/invite-validation-response.model";
import { TokenService } from "../../core/services/token.service";
import { ServerService } from "../../core/services/server.service";

@Component({
  selector: "app-join-server",
  standalone: true,
  imports: [],
  templateUrl: "./join-server.component.html",
  styleUrl: "./join-server.component.css",
})
export class JoinServerComponent implements OnInit {
  constructor(
    private route: ActivatedRoute,
    private tokenService: TokenService,
    private serverService: ServerService,
    private router: Router
  ) {}

  serverJoinToken: string = "";
  response: InviteValidationResponse | undefined = undefined;

  errorMessages: string[] = [];

  ngOnInit(): void {
    this.route.queryParams.subscribe((params) => {
      const token = params["token"];
      if (token) {
        this.tokenService.validateToken(token).subscribe({
          next: (res) => {
            this.response = res;
            this.serverJoinToken = token;
          },
        });
      }
    });
  }

  joinServer() {
    this.serverService.joinServerByToken(this.serverJoinToken).subscribe({
      next: (res) => {
        console.log(res);
        this.router.navigate(["/chat"], { skipLocationChange: true });
      },
      error: (err) => {
        this.errorMessages = err.error.errorMessages;
      },
    });
  }
}
