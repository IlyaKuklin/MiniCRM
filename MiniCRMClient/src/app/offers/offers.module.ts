import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { OffersRoutingModule } from './offers-routing.module';
import { OffersListComponent } from './components/offers-list/offers-list.component';
import { EditOfferComponent } from './components/edit-offer/edit-offer.component';
import { SharedModule } from '../shared/shared.module';


@NgModule({
  declarations: [
    OffersListComponent,
    EditOfferComponent
  ],
  imports: [
    CommonModule,
    OffersRoutingModule,
    SharedModule
  ]
})
export class OffersModule { }
