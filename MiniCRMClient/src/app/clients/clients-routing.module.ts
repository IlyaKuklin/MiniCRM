import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { Role } from 'src/api/rest/api';
import { RolesGuard } from '../shared/guards/roles.guards';
import { ClientsListComponent } from './components/clients-list/clients-list.component';
import { EditClientComponent } from './components/edit-client/edit-client.component';

const routes: Routes = [
  {
    path: 'clients',
    component: ClientsListComponent,
    canActivate: [RolesGuard],
    data: { roles: [Role.NUMBER_2, Role.NUMBER_3] },
  },
  {
    path: 'clients/edit/:id',
    component: EditClientComponent,
    canActivate: [RolesGuard],
    data: { roles: [Role.NUMBER_2, Role.NUMBER_3] },
  },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
})
export class ClientsRoutingModule {}
