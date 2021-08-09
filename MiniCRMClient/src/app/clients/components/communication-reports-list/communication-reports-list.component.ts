import { Component, Input, OnInit } from '@angular/core';
import { MatTableDataSource } from '@angular/material/table';
import { ClientCommunicationReportDto } from 'src/api/rest/api';

@Component({
  selector: 'mcrm-communication-reports-list',
  templateUrl: './communication-reports-list.component.html',
  styleUrls: ['./communication-reports-list.component.scss']
})
export class CommunicationReportsListComponent implements OnInit {

  constructor() { }

  @Input('reports') model: ClientCommunicationReportDto[] = [];
  displayedColumns: string[] = ['text', 'date', 'del'];
  dataSource!: MatTableDataSource<ClientCommunicationReportDto>;

  ngOnInit(): void {

    this.dataSource = new MatTableDataSource<ClientCommunicationReportDto>(this.model);

  }

}
