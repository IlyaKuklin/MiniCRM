import {
  Component,
  EventEmitter,
  Inject,
  OnInit,
  Output,
  ViewChild,
} from '@angular/core';
import { NgForm } from '@angular/forms';
import { MAT_DIALOG_DATA } from '@angular/material/dialog';
import {
  ClientCommunicationReportDto,
  ClientCommunicationReportEditDto,
  ClientsApiService,
} from 'src/api/rest/api';
import { ClientsService } from '../../services/clients.service';
import { ClientErrorStateMatcher } from '../edit-client/edit-client.component';

@Component({
  selector: 'mcrm-edit-communication-report',
  templateUrl: './edit-communication-report.component.html',
  styleUrls: ['./edit-communication-report.component.scss'],
})
export class EditCommunicationReportComponent implements OnInit {
  constructor(
    @Inject(MAT_DIALOG_DATA) public data: ClientCommunicationReportEditDto,
    private readonly clientApiService: ClientsApiService,
    private readonly clientService: ClientsService
  ) {}

  model!: ClientCommunicationReportEditDto;
  @ViewChild('form') form!: NgForm;

  isLoading: boolean = false;
  isEdit: boolean = false;
  // TODO: вынести матчер в отдельный файл
  errorStateMatcher = new ClientErrorStateMatcher();

  ngOnInit(): void {
    this.model = this.data;
    this.isEdit = <ClientCommunicationReportEditDto>this.model.id > 0;

    console.log(this.model);
  }

  submit(): void {
    if (!this.form.valid) return;

    this.clientApiService
      .apiClientsCommunicationReportsEditPost({
        clientId: this.model.clientId,
        id: this.model.id,
        text: this.model.text,
      })
      .subscribe((response: ClientCommunicationReportDto) => {
        this.clientService.communicationReportSubject.next(response);
      });
  }
}
