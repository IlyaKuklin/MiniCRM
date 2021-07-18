import { Injectable } from '@angular/core';
import { HttpInterceptor, HttpHandler, HttpRequest } from '@angular/common/http';
import { AuthService } from 'src/app/auth/services/auth.service';

@Injectable()
export class AuthInterceptor implements HttpInterceptor {
	constructor(private readonly authService: AuthService) {}

	intercept(req: HttpRequest<any>, next: HttpHandler) {
		if (this.authService.isLoggedIn()) {
			const jwtToken = this.authService.getToken();
			let authReq = req.clone({
				setHeaders: { Authorization: 'Bearer ' + jwtToken }
			});

			return next.handle(authReq);
		}

		return next.handle(req);
	}
}
