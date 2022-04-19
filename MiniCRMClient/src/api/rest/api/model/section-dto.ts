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
import { OfferSectionType } from './offer-section-type';


export interface SectionDto { 
    name: string;
    data: string;
    imagePaths: Array<string>;
    type: string;
    type2: OfferSectionType;
}

