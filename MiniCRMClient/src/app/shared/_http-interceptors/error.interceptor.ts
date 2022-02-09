import { Injectable } from '@angular/core';
import {
  HttpInterceptor,
  HttpHandler,
  HttpRequest,
  HttpEvent,
  HttpErrorResponse,
} from '@angular/common/http';
import { Observable } from 'rxjs';
import { tap } from 'rxjs/operators';
import { SnackbarService } from '../services/snackbar.service';

@Injectable()
export class ErrorInterceptor implements HttpInterceptor {
  constructor(private readonly snackbarService: SnackbarService) {}

  intercept(
    req: HttpRequest<any>,
    next: HttpHandler
  ): Observable<HttpEvent<any>> {
    return next.handle(req).pipe(
      tap(
        (event) => {},
        (error) => {
          if (error instanceof HttpErrorResponse) {
            alert(error.error.Message);
            this.snackbarService.show({
                duration: 3000,
                message: error.error.Message,
                isError: true
            })
          }
        }
      )
    );
  }
}
