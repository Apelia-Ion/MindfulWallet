import { HttpErrorResponse, HttpInterceptorFn } from '@angular/common/http';
import { AuthService } from '../services/auth.service';
import { inject } from '@angular/core';
import { Router } from '@angular/router';
import { catchError, throwError } from 'rxjs';

export const tokenInterceptor: HttpInterceptorFn = (req, next) => {
  const authService = inject(AuthService);
  const myToken = authService.getToken();
  const router =inject(Router);

  console.log('Interceptor activated'); // Debugging line
  console.log(`Token found: ${myToken}`); // Debugging line

  if (myToken) {
    req = req.clone({
      setHeaders: { Authorization: `Bearer ${myToken}` }
    });
  }
  return next(req).pipe(
    catchError((err: any) => {
      if (err instanceof HttpErrorResponse) {
        if (err.status === 401) {
          alert('Token is expired, Please Login again');
          router.navigate(['login']);
        }
      }
      return throwError(() => new Error('Some other error occurred'));
    })
  );
};
