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
import { OfferPotential } from './offer-potential';
import { OfferFileDatum } from './offer-file-datum';
import { Client } from './client';


export interface Offer { 
    id?: number;
    created?: string;
    changed?: string;
    number?: number;
    productSystemType?: string | null;
    briefIndustryDescription?: string | null;
    offerCase?: string | null;
    description?: string | null;
    offerPoint?: string | null;
    recommendations?: string | null;
    otherDocumentation?: string | null;
    coveringLetter?: string | null;
    similarCases?: string | null;
    newsLinks?: string | null;
    potential?: OfferPotential;
    stage?: string | null;
    client?: Client;
    clientId?: number;
    fileData?: Array<OfferFileDatum> | null;
    selectedSections?: Array<string> | null;
}

