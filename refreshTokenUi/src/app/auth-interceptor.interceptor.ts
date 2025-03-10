import { HttpInterceptorFn } from '@angular/common/http';
import { AuthServiceService } from './auth-service.service';
import { inject } from '@angular/core';
import { catchError, of, switchMap } from 'rxjs';

export const authInterceptorInterceptor: HttpInterceptorFn = (req, next) => {

  const authService = inject(AuthServiceService);
  const accessToken = authService.getAccessToken();
  const refreshToken = authService.getRefreshToken();

  if (accessToken) {
    const clonedReq = req.clone({
      setHeaders: {
        Authorization: `Bearer ${accessToken}`
      }
    });

    return next(clonedReq).pipe(
      catchError((error) => {
        if (error.status === 401 && refreshToken) {
          // Handle refresh token
          return authService.refreshToken(accessToken, refreshToken).pipe(
            switchMap((response) => {
              const newAccessToken = response.accessToken;
              const newRefreshToken = response.refreshToken;
              authService.storeTokens(newAccessToken, newRefreshToken);

              const clonedReq = req.clone({
                setHeaders: {
                  Authorization: `Bearer ${newAccessToken}`
                }
              });

              return next(clonedReq);
            })
          );
        } else {
          return of(error);
        }
      })
    );
  }

  return next(req);
};
