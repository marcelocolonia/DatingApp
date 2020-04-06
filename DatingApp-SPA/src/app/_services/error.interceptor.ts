import { Injectable, Provider } from '@angular/core';
import {
  HttpInterceptor,
  HttpRequest,
  HttpHandler,
  HttpEvent,
  HttpErrorResponse,
  HTTP_INTERCEPTORS
} from '@angular/common/http';
import { Observable, throwError } from 'rxjs';
import { catchError } from 'rxjs/operators';

@Injectable()
export class ErrorInterceptor implements HttpInterceptor {
  intercept(
    req: HttpRequest<any>,
    next: HttpHandler
  ): Observable<HttpEvent<any>> {
    return next.handle(req).pipe(
      catchError(errorResponse => {
        if (errorResponse.status === 401) {
          return throwError(errorResponse.statusText);
        }

        //  500 errors
        if (errorResponse instanceof HttpErrorResponse) {
          const applicationError = errorResponse.headers.get(
            'Application-Error'
          );
          if (applicationError) {
            return throwError(applicationError);
          }

          const serverError = errorResponse.error;
          let modalStateErrors = '';

          if (serverError.errors && typeof serverError.errors === 'object') {
            for (const key in serverError.errors) {
              if (serverError.errors[key]) {
                modalStateErrors += serverError.errors[key] + '\n';
              }
            }
          }

          return throwError(modalStateErrors || serverError || 'Server error');
        }
      })
    );
  }
}

export const ErrorInterceptorProvider: Provider = {
  provide: HTTP_INTERCEPTORS,
  useClass: ErrorInterceptor,
  multi: true
};
