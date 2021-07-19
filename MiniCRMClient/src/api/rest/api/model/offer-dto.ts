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
import { ClientDto } from './client-dto';


export interface OfferDto { 
    id?: number;
    number?: number;
    productSystemType?: string | null;
    briefIndustryDescription?: string | null;
    _case?: string | null;
    client?: ClientDto;
    clientId?: number;
}
