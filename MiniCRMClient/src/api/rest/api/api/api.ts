export * from './auth.service';
import { AuthApiService } from './auth.service';
export * from './clients.service';
import { ClientsApiService } from './clients.service';
export * from './common.service';
import { CommonApiService } from './common.service';
export * from './offers.service';
import { OffersApiService } from './offers.service';
export const APIS = [AuthApiService, ClientsApiService, CommonApiService, OffersApiService];
