import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { AuthRoutingModule } from './auth-routing.module';
import { LoginComponent } from './components/login/login.component';
import { SharedModule } from '../shared/shared.module';
import { ManagersListComponent } from './components/managers-list/managers-list.component';
import { EditManagerComponent } from './components/edit-manager/edit-manager.component';
import { EmailSettingsComponent } from './components/email-settings/email-settings.component';

@NgModule({
  declarations: [LoginComponent, ManagersListComponent, EditManagerComponent, EmailSettingsComponent],
  imports: [CommonModule, AuthRoutingModule, SharedModule],
})
export class AuthModule {}
