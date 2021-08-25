import { Component, OnInit, ViewChild } from '@angular/core';
import { MatPaginator } from '@angular/material/paginator';
import { MatTableDataSource } from '@angular/material/table';
import { Subject } from 'rxjs';
import { debounceTime, distinctUntilChanged, mergeMap } from 'rxjs/operators';
import { OfferDto, OffersApiService } from 'src/api/rest/api';
import { DialogService } from 'src/app/shared/services/dialog.service';
import { SnackbarService } from 'src/app/shared/services/snackbar.service';

@Component({
  selector: 'mcrm-offers-list',
  templateUrl: './offers-list.component.html',
  styleUrls: ['./offers-list.component.scss'],
})
export class OffersListComponent implements OnInit {
  constructor(
    private readonly offersApiService: OffersApiService,
    private readonly snackbarService: SnackbarService,
    private readonly dialogService: DialogService
  ) {}

  model: OfferDto[] = [];
  displayedColumns: string[] = ['id', 'number', 'clientName', 'del'];
  dataSource!: MatTableDataSource<OfferDto>;
  resultsLength: number = 0;
  isLoading = true;

  @ViewChild(MatPaginator) paginator!: MatPaginator;

  private searchText$ = new Subject<string>();

  ngOnInit(): void {
    this.offersApiService.apiOffersListGet().subscribe((response) => {
      this.model = response;
      this.dataSource = new MatTableDataSource<OfferDto>(response);
      this.dataSource.paginator = this.paginator;
      this.isLoading = false;

      this.searchText$
        .pipe(
          debounceTime(500),
          distinctUntilChanged(),
          mergeMap((filter) => {
            this.isLoading = true;
            return this.offersApiService.apiOffersListGet(filter);
          })
        )
        .subscribe((response) => {
          this.model = response;
          this.dataSource = new MatTableDataSource<OfferDto>(response);
          this.isLoading = false;
        });
    });
  }

  delete(id: number) {
    this.dialogService
      .confirmDialog({
        header: 'Удаление',
        message: 'Вы уверены, что хотите удалить КП?',
      })
      .subscribe((result) => {
        if (result) {
          this.isLoading = true;
          this.offersApiService
            .apiOffersDeleteDelete(id)
            .subscribe((response) => {
              this.model = this.model.filter((x) => x.id !== id);
              this.dataSource = new MatTableDataSource<OfferDto>(this.model);
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
