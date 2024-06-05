import { HttpErrorResponse, HttpHandlerFn, HttpInterceptorFn, HttpRequest } from '@angular/common/http';
import { AuthService } from '../services/auth.service';
import { inject } from '@angular/core';
import { Router } from '@angular/router';
import { catchError, switchMap, throwError, Observable, of } from 'rxjs';
import { TokenApiModel } from '../models/token-api.model';

export const tokenInterceptor: HttpInterceptorFn = (req, next) => {
  const authService = inject(AuthService);
  const myToken = authService.getToken();
  const router = inject(Router);

  if (myToken) {
    req = req.clone({
      setHeaders: { Authorization: `Bearer ${myToken}` }
    });
  }

  return next(req).pipe(
    catchError((err: any) => {
      if (err instanceof HttpErrorResponse) {
        if (err.status === 401) {
          console.log("401 Unauthorized error caught"); // Debugging line
          return handleUnAuthorisedError(req, next, authService, router);
        }
      }
      return throwError(() => new Error('Some other error occurred'));
    })
  );
};

function handleUnAuthorisedError(req: HttpRequest<any>, next: HttpHandlerFn, authService: AuthService, router: Router): Observable<any> {
  const tokenApiModel = new TokenApiModel();
  tokenApiModel.accessToken = authService.getToken()!;
  tokenApiModel.refreshToken = authService.getRefreshToken()!;

  return authService.renewToken(tokenApiModel).pipe(
    switchMap((data: TokenApiModel) => {
      authService.storeRefreshToken(data.refreshToken);
      authService.storeToken(data.accessToken);

      const clonedRequest = req.clone({
        setHeaders: { Authorization: `Bearer ${data.accessToken}` }
      });
      return next(clonedRequest);
    }),
    catchError((err) => {
      alert('Token is expired, Please Login again');
      router.navigate(['login']);
      return throwError(() => new Error('Token refresh failed'));
    })
  );
}
