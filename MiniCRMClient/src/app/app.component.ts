import { Component } from '@angular/core';
import { AuthService } from './auth/services/auth.service';

@Component({
  selector: 'mcrm-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss'],
})
export class AppComponent {
  title = 'Автомат Сервис CRM';

  constructor(private readonly authService: AuthService) {}

  get showToolbar(): boolean {
    const userData = this.authService.getUserData();
    const uuidRegex =
      /\b[0-9a-f]{8}\b-[0-9a-f]{4}-[0-9a-f]{4}-[0-9a-f]{4}-\b[0-9a-f]{12}\b/;
    const isOfferView = uuidRegex.test(window.location.href) && !userData;
    return !isOfferView;
  }
}
