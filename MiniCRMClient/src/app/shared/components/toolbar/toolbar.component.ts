import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { UserAuthResponseDto } from 'src/api/rest/api';
import { AuthService } from 'src/app/auth/services/auth.service';

@Component({
  selector: 'mcrm-toolbar',
  templateUrl: './toolbar.component.html',
  styleUrls: ['./toolbar.component.scss'],
})
export class ToolbarComponent implements OnInit {
  constructor(
    private readonly router: Router,
    private readonly authService: AuthService
  ) {
    this.userData = this.authService.getUserData();
    this.authService.userDataSubject.subscribe(
      (userData) => (this.userData = userData)
    );
  }

  userData: UserAuthResponseDto | null = null;

  ngOnInit(): void {}

  get userName(): string | null | undefined {
    return this.userData ? this.userData.name : '';
  }

  goToLogin() {
    this.router.navigate(['/login']);
  }

  goToHomePage(): void {
    this.router.navigate(['/']);
  }

  isLoggedIn(): boolean {
    return !!this.userData;
  }

  logout(): void {
    this.authService.clearUserData();
    this.goToLogin();
  }

  goTo(path: string): void {
    this.router.navigate([path]);
  }
}
