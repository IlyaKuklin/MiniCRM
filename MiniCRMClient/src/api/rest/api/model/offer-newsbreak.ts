/**
 * CRM API
 * CRM API Reference
 *
 * The version of the OpenAPI document: v1
 * 
 *
 * NOTE: This class is auto generated by OpenAPI Generator (https://openapi-generator.tech).
 * https://openapi-generator.tech
 * Do not edit the class manually.
 */
import { User } from './user';
import { Offer } from './offer';


export interface OfferNewsbreak { 
    id?: number;
    created?: string;
    changed?: string;
    offer?: Offer;
    offerId?: number;
    text?: string | null;
    author?: User;
    authorId?: number;
}

