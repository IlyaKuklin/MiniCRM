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
import { CommunicationReportDto } from './communication-report-dto';
import { OfferDto } from './offer-dto';


export interface ClientDto { 
    id: number;
    name?: string | null;
    legalEntitiesNames?: string | null;
    domainNames?: string | null;
    contact?: string | null;
    diagnostics?: string | null;
    offers?: Array<OfferDto> | null;
    commonCommunicationReports: Array<CommunicationReportDto>;
}

