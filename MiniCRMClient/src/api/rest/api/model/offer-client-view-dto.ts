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
import { SectionDto } from './section-dto';
import { OfferFeedbackRequestDto } from './offer-feedback-request-dto';


export interface OfferClientViewDto { 
    number: number;
    changed: string;
    managerEmail: string;
    sections: Array<SectionDto>;
    client: ClientDto;
    feedbackRequests: Array<OfferFeedbackRequestDto>;
}

