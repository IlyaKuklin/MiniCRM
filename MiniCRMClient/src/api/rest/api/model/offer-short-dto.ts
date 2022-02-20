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
import { OfferVersionShortDto } from './offer-version-short-dto';


export interface OfferShortDto { 
    id?: number;
    created?: string;
    changed?: string;
    number?: number;
    clientId?: number;
    clientName?: string | null;
    currentVersionNumber?: number;
    clientVersionNumber?: number;
    status?: string | null;
    versions?: Array<OfferVersionShortDto> | null;
}

