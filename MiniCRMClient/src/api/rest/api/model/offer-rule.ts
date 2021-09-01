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
import { Offer } from './offer';


export interface OfferRule { 
    id?: number;
    created?: string;
    changed?: string;
    offer?: Offer;
    offerId?: number;
    completed?: boolean;
    task?: string | null;
    report?: string | null;
    deadline?: string;
}

