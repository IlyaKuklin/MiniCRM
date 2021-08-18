import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { Role } from 'src/api/rest/api';
import { RolesGuard } from '../shared/guards/roles.guards';
import { ClientOfferViewComponent } from './components/client-offer-view/client-offer-view.component';
import { EditOfferComponent } from './components/edit-offer/edit-offer.component';
import { OffersListComponent } from './components/offers-list/offers-list.component';

const routes: Routes = [
  {
    path: 'offers',
    component: OffersListComponent,
    canActivate: [RolesGuard],
    data: { roles: [Role.NUMBER_2, Role.NUMBER_3] },
  },
  {
    path: 'offers/edit/:id',
    component: EditOfferComponent,
    canActivate: [RolesGuard],
    data: { roles: [Role.NUMBER_2, Role.NUMBER_3] },
  },
  {
    path: 'offers/:clientOfferId',
    component: ClientOfferViewComponent,
  },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
})
export class OffersRoutingModule {}
