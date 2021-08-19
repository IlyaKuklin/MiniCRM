import {
  AfterViewInit,
  Component,
  Input,
  OnInit,
  ViewChild,
} from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { MatPaginator } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';
import {
  //ClientCommunicationReportDto,
  //ClientCommunicationReportEditDto,
  //ClientsApiService,
  CommonApiService,
  CommunicationReportDto,
} from 'src/api/rest/api';
import { DialogService } from 'src/app/shared/services/dialog.service';
import { SnackbarService } from 'src/app/shared/services/snackbar.service';
import { ClientsService } from '../../../clients/services/clients.service';
import { EditCommunicationReportComponent } from '../../../clients/components/edit-communication-report/edit-communication-report.component';

@Component({
  selector: 'mcrm-communication-reports-list',
  templateUrl: './communication-reports-list.component.html',
  styleUrls: ['./communication-reports-list.component.scss'],
})
export class CommunicationReportsListComponent
  implements OnInit, AfterViewInit
{
  constructor(
    private readonly matDialog: MatDialog,
    //private readonly clientsApiService: ClientsApiService,
    private readonly clientsService: ClientsService,
    private readonly snackbarService: SnackbarService,
    private readonly dialogService: DialogService,
    private readonly commonApiService: CommonApiService
  ) {}

  @Input('id') id!: number | undefined;
  @Input('reports') model: CommunicationReportDto[] = [];
  @Input('type') type: string = 'client';

  displayedColumns: string[] = ['date', 'author', 'text', 'del'];
  dataSource!: MatTableDataSource<CommunicationReportDto>;
  @ViewChild(MatPaginator) paginator!: MatPaginator;
  @ViewChild(MatSort) sort!: MatSort;

  ngOnInit(): void {
    this.refreshDataSource();

    this.clientsService.communicationReportSubject.subscribe((reportDto) => {
      this.onEdit(reportDto);
    });
  }

  ngAfterViewInit(): void {
    this.dataSource.paginator = this.paginator;
    this.dataSource.sort = this.sort;
  }

  onEdit(dto: CommunicationReportDto): void {
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
    const newReportEdit: CommunicationReportDto = {
      id: 0,
      text: '',
      clientId: -1,
      offerId: -1,
    };

    if (this.type == 'client') {
      newReportEdit.clientId = this.id;
    } else if (this.type == 'offer') {
      newReportEdit.offerId = this.id;
    } else {
      throw new Error('not implemented');
    }

    const dialogRef = this.matDialog.open(EditCommunicationReportComponent, {
      data: newReportEdit,
    });
  }

  editClick(report: CommunicationReportDto): void {
    const editDto: CommunicationReportDto = {
      //clientId: this.clientId,
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
          this.commonApiService
            .apiCommonCommunicationReportsDeleteDelete(id)
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

  getText(report: CommunicationReportDto) {
    if (report.text && report.text?.length <= 150) return report.text;
    return `${report.text?.substring(0, 150)}...`;
  }

  private refreshDataSource() {
    this.dataSource = new MatTableDataSource<CommunicationReportDto>(
      this.model
    );
    this.dataSource.paginator = this.paginator;
  }
}
