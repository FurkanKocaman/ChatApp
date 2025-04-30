import { inject } from "@angular/core";
import { CanActivateFn, Router } from "@angular/router";
import { switchMap, take, tap, of, map } from "rxjs";
import { UserService } from "../services/user.service";

export const authGuard: CanActivateFn = (route, state) => {
  const userService = inject(UserService);
  const router = inject(Router);

  return userService.user$.pipe(
    take(1),
    switchMap((user) => {
      if (user) {
        return of(true);
      }

      return userService.getCurrentUser().pipe(
        tap((currentUser) => {
          if (!currentUser) {
            router.navigate(["/auth/login"], {
              skipLocationChange: true,
              queryParams: { returnUrl: state.url },
            });
          }
        }),
        map((currentUser) => !!currentUser)
      );
    })
  );
};
