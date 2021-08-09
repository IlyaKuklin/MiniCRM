import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { ClientsRoutingModule } from './clients-routing.module';
import { ClientsListComponent } from './components/clients-list/clients-list.component';
import { SharedModule } from '../shared/shared.module';
import { EditClientComponent } from './components/edit-client/edit-client.component';
import { CommunicationReportsListComponent } from './components/communication-reports-list/communication-reports-list.component';


@NgModule({
  declarations: [
    ClientsListComponent,
    EditClientComponent,
    CommunicationReportsListComponent
  ],
  imports: [
    CommonModule,
    ClientsRoutingModule,
    SharedModule
  ]
})
export class ClientsModule { }
