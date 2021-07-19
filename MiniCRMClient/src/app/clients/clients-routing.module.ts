import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { ClientsListComponent } from './components/clients-list/clients-list.component';
import { EditClientComponent } from './components/edit-client/edit-client.component';

const routes: Routes = [
  {
    path: 'clients',
    component: ClientsListComponent,
  },
  {
    path: 'clients/edit/:id',
    component: EditClientComponent,
  },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
})
export class ClientsRoutingModule {}
