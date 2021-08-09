import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { MatTableDataSource } from '@angular/material/table';
import {
  ClientCommunicationReportDto,
  ClientCommunicationReportEditDto,
} from 'src/api/rest/api';
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
    private readonly clientService: ClientsService
  ) {}

  @Input('clientId') clientId!: number;
  @Input('reports') model: ClientCommunicationReportDto[] = [];
  displayedColumns: string[] = ['text', 'date', 'del'];
  dataSource!: MatTableDataSource<ClientCommunicationReportDto>;

  ngOnInit(): void {
    this.refreshDataSource();

    this.clientService.communicationReportSubject.subscribe((reportDto) => {
      this.onEdit(reportDto);
    });
  }

  onEdit(dto: ClientCommunicationReportDto): void {
    this.matDialog.closeAll();
    const report = this.model.find((x) => x.id == dto.id);
    if (!report) {
      this.model.push(dto);
      this.refreshDataSource();
    }
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

    dialogRef.afterClosed().subscribe((result) => {
      console.log(`Dialog result: ${result}`);
    });
  }

  private refreshDataSource() {
    this.dataSource = new MatTableDataSource<ClientCommunicationReportDto>(
      this.model
    );
  }
}
