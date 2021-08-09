import { Injectable } from '@angular/core';
import { Subject } from 'rxjs';
import { ClientCommunicationReportDto } from 'src/api/rest/api';

@Injectable({
  providedIn: 'root',
})
export class ClientsService {
  communicationReportSubject = new Subject<ClientCommunicationReportDto>();
}
