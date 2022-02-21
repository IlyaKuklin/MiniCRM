import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { OffersRoutingModule } from './offers-routing.module';
import { OffersListComponent } from './components/offers-list/offers-list.component';
import { EditOfferComponent } from './components/edit-offer/edit-offer.component';
import { SharedModule } from '../shared/shared.module';
import { ClientOfferViewComponent } from './components/client-offer-view/client-offer-view.component';
import { OffersCheckListComponent } from './components/offers-check-list/offers-check-list.component';


@NgModule({
  declarations: [
    OffersListComponent,
    EditOfferComponent,
    ClientOfferViewComponent,
    OffersCheckListComponent
  ],
  imports: [
    CommonModule,
    OffersRoutingModule,
    SharedModule
  ]
})
export class OffersModule { }
