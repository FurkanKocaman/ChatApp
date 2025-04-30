import { HttpEvent, HttpRequest, HttpHandlerFn } from "@angular/common/http";
import { inject } from "@angular/core";
import { Observable } from "rxjs";
import { ServerService } from "../services/server.service";

export const JWTInterceptor = (
  req: HttpRequest<any>,
  next: HttpHandlerFn
): Observable<HttpEvent<any>> => {
  const serverService = inject(ServerService);

  const serverId = serverService.getCurrentServerSnapshot()?.id ?? "";

  if (localStorage.getItem("access_token")) {
    req = req.clone({
      setHeaders: {
        Authorization: `Bearer ${localStorage.getItem("access_token")}`,
        "X-Current-Server": serverId,
      },
    });
  }

  return next(req);
};
