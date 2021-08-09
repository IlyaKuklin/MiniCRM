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
import { UserDto } from './user-dto';


export interface ClientCommunicationReportDto { 
    id?: number;
    created?: string;
    changed?: string;
    text?: string | null;
    author?: UserDto;
    client?: ClientDto;
}

