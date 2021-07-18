import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { Role } from 'src/api/rest/api';
import { RolesGuard } from '../guards/roles.guards';
import { EditManagerComponent } from './components/edit-manager/edit-manager.component';
import { LoginComponent } from './components/login/login.component';
import { ManagersListComponent } from './components/managers-list/managers-list.component';

const routes: Routes = [
  {
    path: 'login',
    component: LoginComponent,
  },
  {
    path: 'managers',
    component: ManagersListComponent,
    canActivate: [RolesGuard],
    data: { roles: [Role.NUMBER_2] },
  },
  {
    path: 'managers/edit/:id',
    component: EditManagerComponent,
    canActivate: [RolesGuard],
    data: { roles: [Role.NUMBER_2] },
  },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
})
export class AuthRoutingModule {}
