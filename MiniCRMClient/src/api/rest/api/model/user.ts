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
import { Role } from './role';


export interface User { 
    id?: number;
    login?: string | null;
    passwordHash?: string | null;
    salt?: string;
    isDeleted?: boolean;
    role?: Role;
    name?: string | null;
    email?: string | null;
}

