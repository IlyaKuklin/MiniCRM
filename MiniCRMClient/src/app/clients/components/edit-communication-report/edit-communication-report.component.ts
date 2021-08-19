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
  CommonApiService,
  CommunicationReportDto,
  CommunicationReportEditDto,
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
    @Inject(MAT_DIALOG_DATA) public data: CommunicationReportEditDto,
    private readonly commonApiService: CommonApiService,
    private readonly clientService: ClientsService
  ) {}

  model!: CommunicationReportDto;
  @ViewChild('form') form!: NgForm;

  isLoading: boolean = false;
  isEdit: boolean = false;
  // TODO: вынести матчер в отдельный файл
  errorStateMatcher = new ClientErrorStateMatcher();

  ngOnInit(): void {
    console.log(this.data)
    this.model = {...this.data};
    this.isEdit = <CommunicationReportDto>this.model.id > 0;
  }

  submit(): void {
    if (!this.form.valid) return;

    this.commonApiService
      .apiCommonCommunicationReportsEditPost({
        //clientId: this.model.clientId,
        id: <number>this.model.id,
        text: <string>this.model.text,
        clientId: <number>this.model.clientId,
        offerId: <number>this.model.offerId
      })
      .subscribe((response: CommunicationReportDto) => {
        this.clientService.communicationReportSubject.next(response);
      });
  }
}
