import { Injectable } from '@angular/core';
import {
  ActivatedRouteSnapshot,
  CanActivate,
  Router,
  RouterStateSnapshot,
  UrlTree,
} from '@angular/router';
import { Observable } from 'rxjs';
import { Role } from 'src/api/rest/api';
import { AuthService } from '../../auth/services/auth.service';

@Injectable({
  providedIn: 'root',
})
export class RolesGuard implements CanActivate {
  constructor(private router: Router, private authService: AuthService) {}

  canActivate(
    route: ActivatedRouteSnapshot,
    state: RouterStateSnapshot
  ):
    | Observable<boolean | UrlTree>
    | Promise<boolean | UrlTree>
    | boolean
    | UrlTree {
    const requiredRoles = <Array<Role>>route.data['roles'];
    const userData = this.authService.getUserData();
    const canActivate =
      userData != null &&
      userData.role != null &&
      requiredRoles.indexOf(userData.role) > -1;

    if (!canActivate) this.router.navigate(['/forbidden']);

    return true;
  }
}
