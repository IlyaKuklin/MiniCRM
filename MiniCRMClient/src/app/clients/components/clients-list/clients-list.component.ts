import { Component, OnInit, ViewChild } from '@angular/core';
import { MatPaginator } from '@angular/material/paginator';
import { MatTableDataSource } from '@angular/material/table';
import { merge, Observable, Subject } from 'rxjs';
import { ClientDto, ClientsApiService } from 'src/api/rest/api';
import {
  debounceTime,
  distinctUntilChanged,
  mergeMap,
  switchMap,
} from 'rxjs/operators';
import { DialogService } from 'src/app/shared/services/dialog.service';
import { SnackbarService } from 'src/app/shared/services/snackbar.service';

@Component({
  selector: 'mcrm-clients-list',
  templateUrl: './clients-list.component.html',
  styleUrls: ['./clients-list.component.scss'],
})
export class ClientsListComponent implements OnInit {
  constructor(
    private readonly clientsApiService: ClientsApiService,
    private readonly snackbarService: SnackbarService,
    private readonly dialogService: DialogService
  ) {}

  model: ClientDto[] = [];
  displayedColumns: string[] = ['id', 'name', 'del'];
  dataSource!: MatTableDataSource<ClientDto>;
  resultsLength: number = 0;
  isLoading = true;

  @ViewChild(MatPaginator) paginator!: MatPaginator;

  private searchText$ = new Subject<string>();

  ngOnInit(): void {
    this.clientsApiService.apiClientsListGet().subscribe((response) => {
      this.model = response;
      console.log(response)
      this.dataSource = new MatTableDataSource<ClientDto>(response);
      this.dataSource.paginator = this.paginator;
      this.isLoading = false;

      this.searchText$
        .pipe(
          debounceTime(500),
          distinctUntilChanged(),
          mergeMap((filter) => {
            this.isLoading = true;
            return this.clientsApiService.apiClientsListGet(filter);
          })
        )
        .subscribe((response) => {
          this.model = response;
          this.dataSource = new MatTableDataSource<ClientDto>(response);
          this.isLoading = false;
        });
    });
  }

  delete(client: ClientDto) {
    this.dialogService
      .confirmDialog({
        header: 'Удаление клиента',
        message: `Вы уверены, что хотите удалить клиента ${client.name}?`,
      })
      .subscribe((result: boolean) => {
        if (result) {
          this.isLoading = true;
          this.clientsApiService
            .apiClientsDeleteDelete(client.id)
            .subscribe((response) => {
              this.model = this.model.filter((x) => x.id !== client.id);
              this.dataSource = new MatTableDataSource<ClientDto>(this.model);
              this.snackbarService.show({
                message: 'Клиент удалён',
                duration: 3000,
              });
              this.isLoading = false;
            });
        }
      });
  }

  applyFilter(event: KeyboardEvent) {
    const value = (event.target as HTMLInputElement).value;
    this.searchText$.next(value);
  }
}
