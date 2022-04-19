import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { Role } from 'src/api/rest/api';
import { RolesGuard } from '../shared/guards/roles.guards';
import { ClientOfferViewComponent } from './components/client-offer-view/client-offer-view.component';
import { EditOfferComponent } from './components/edit-offer/edit-offer.component';
import { OffersCheckListComponent } from './components/offers-check-list/offers-check-list.component';
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
  {
    path: 'offers/:clientOfferId/:key',
    component: ClientOfferViewComponent,
  },
  {
    path: 'offers/checkRules',
    component: OffersCheckListComponent,
    canActivate: [RolesGuard],
    data: { roles: [Role.Administrator] },
  },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
})
export class OffersRoutingModule {}
