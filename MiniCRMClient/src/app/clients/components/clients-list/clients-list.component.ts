import { Component, OnInit, ViewChild } from '@angular/core';
import { MatPaginator } from '@angular/material/paginator';
import { MatTableDataSource } from '@angular/material/table';
import { ClientDto, ClientsApiService } from 'src/api/rest/api';

@Component({
  selector: 'mcrm-clients-list',
  templateUrl: './clients-list.component.html',
  styleUrls: ['./clients-list.component.scss'],
})
export class ClientsListComponent implements OnInit {
  constructor(private readonly clientsApiService: ClientsApiService) {}

  model: ClientDto[] = [];
  displayedColumns: string[] = ['id', 'name', 'del'];
  dataSource!: MatTableDataSource<ClientDto>;
  resultsLength: number = 0;
  isLoading = true;

  @ViewChild(MatPaginator) paginator!: MatPaginator;

  ngOnInit(): void {
    this.clientsApiService.apiClientsListGet().subscribe((response) => {
      this.model = response;
      this.dataSource = new MatTableDataSource<ClientDto>(response);
      this.dataSource.paginator = this.paginator;
      this.isLoading = false;
    });
  }

  delete(id: number) {
    this.isLoading = true;
    this.clientsApiService.apiClientsDeleteDelete(id).subscribe((response) => {
      this.model = this.model.filter((x) => x.id !== id);
      this.dataSource = new MatTableDataSource<ClientDto>(this.model);
      this.isLoading = false;
    });
  }
}
