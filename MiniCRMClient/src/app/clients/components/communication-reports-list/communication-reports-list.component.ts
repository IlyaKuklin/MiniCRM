import { Component, Input, OnInit } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { MatTableDataSource } from '@angular/material/table';
import {
  ClientCommunicationReportDto,
  ClientCommunicationReportEditDto,
  ClientsApiService,
} from 'src/api/rest/api';
import { DialogService } from 'src/app/shared/services/dialog.service';
import { SnackbarService } from 'src/app/shared/services/snackbar.service';
import { ClientsService } from '../../services/clients.service';
import { EditCommunicationReportComponent } from '../edit-communication-report/edit-communication-report.component';

@Component({
  selector: 'mcrm-communication-reports-list',
  templateUrl: './communication-reports-list.component.html',
  styleUrls: ['./communication-reports-list.component.scss'],
})
export class CommunicationReportsListComponent implements OnInit {
  constructor(
    private readonly matDialog: MatDialog,
    private readonly clientsApiService: ClientsApiService,
    private readonly clientsService: ClientsService,
    private readonly snackbarService: SnackbarService,
    private readonly dialogService: DialogService
  ) {}

  @Input('clientId') clientId!: number;
  @Input('reports') model: ClientCommunicationReportDto[] = [];
  displayedColumns: string[] = ['date', 'author', 'text', 'del'];
  dataSource!: MatTableDataSource<ClientCommunicationReportDto>;

  ngOnInit(): void {
    this.refreshDataSource();

    this.clientsService.communicationReportSubject.subscribe((reportDto) => {
      this.onEdit(reportDto);
    });
  }

  onEdit(dto: ClientCommunicationReportDto): void {
    this.matDialog.closeAll();
    let report = this.model.find((x) => x.id == dto.id);
    if (!report) {
      this.model.push(dto);
    } else {
      report.text = dto.text;
    }
    this.refreshDataSource();
  }

  addClick(): void {
    const newReportEdit: ClientCommunicationReportEditDto = {
      clientId: this.clientId,
      id: 0,
      text: '',
    };
    const dialogRef = this.matDialog.open(EditCommunicationReportComponent, {
      data: newReportEdit,
    });
  }

  editClick(report: ClientCommunicationReportDto): void {
    const editDto: ClientCommunicationReportEditDto = {
      clientId: this.clientId,
      id: report.id,
      text: report.text,
    };
    this.matDialog.open(EditCommunicationReportComponent, {
      data: editDto,
    });
  }

  delete(id: number): void {
    this.dialogService
      .confirmDialog({
        header: 'Удаление отчёта',
        message: 'Вы действительно хотите удалить отчёт?',
      })
      .subscribe((result: boolean) => {
        if (result) {
          this.clientsApiService
            .apiClientsCommunicationReportsDeleteDelete(id)
            .subscribe(() => {
              this.model = this.model.filter((x) => x.id !== id);
              this.refreshDataSource();
              this.snackbarService.show({
                message: 'Отчёт удалён',
                duration: 3000,
              });
            });
        }
      });
  }

  getText(report: ClientCommunicationReportDto) {
    if (report.text && report.text?.length <= 150) return report.text;
    return `${report.text?.substring(0, 150)}...`;
  }

  private refreshDataSource() {
    this.dataSource = new MatTableDataSource<ClientCommunicationReportDto>(
      this.model
    );
  }
}
