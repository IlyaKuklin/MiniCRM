import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { SharedModule } from './shared/shared.module';
import { AuthModule } from './auth/auth.module';
import { BASE_PATH } from 'src/api/rest/api';
import { environment } from 'src/environments/environment';
import { httpInterceptorProviders } from './shared/_http-interceptors';
import { ClientsModule } from './clients/clients.module';
import { OffersModule } from './offers/offers.module';

@NgModule({
  declarations: [AppComponent],
  imports: [
    BrowserModule,
    BrowserAnimationsModule,
    SharedModule,
    AuthModule,

    ClientsModule,
    OffersModule,

    //Всегда последний
    AppRoutingModule,
  ],
  providers: [
    { provide: BASE_PATH, useValue: environment.basePath },
    httpInterceptorProviders,
  ],
  bootstrap: [AppComponent],
})
export class AppModule {}
