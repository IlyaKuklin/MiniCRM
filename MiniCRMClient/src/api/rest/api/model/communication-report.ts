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


export interface CommunicationReport { 
    id?: number;
    created?: string;
    changed?: string;
    text?: string | null;
    authorId?: number;
    author?: User;
}
