import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { Role } from 'src/api/rest/api';
import { RolesGuard } from '../shared/guards/roles.guards';
import { EditOfferComponent } from './components/edit-offer/edit-offer.component';
import { OffersListComponent } from './components/offers-list/offers-list.component';

const routes: Routes = [
  {
    path: 'offers',
    component: OffersListComponent,
    canActivate: [RolesGuard],
    data: { roles: [Role.Administrator, Role.Manager] },
  },
  {
    path: 'offers/edit/:id',
    component: EditOfferComponent,
    canActivate: [RolesGuard],
    data: { roles: [Role.Administrator, Role.Manager] },
  },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
})
export class OffersRoutingModule {}
