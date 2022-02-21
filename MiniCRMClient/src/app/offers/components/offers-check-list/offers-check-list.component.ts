import { Component, OnInit, ViewChild } from '@angular/core';
import { MatPaginator } from '@angular/material/paginator';
import { MatTableDataSource } from '@angular/material/table';
import { Subject } from 'rxjs';
import { OfferRuleDto, OffersApiService } from 'src/api/rest/api';
import { DialogService } from 'src/app/shared/services/dialog.service';
import { SnackbarService } from 'src/app/shared/services/snackbar.service';

@Component({
  selector: 'mcrm-offers-check-list',
  templateUrl: './offers-check-list.component.html',
  styleUrls: ['./offers-check-list.component.scss'],
})
export class OffersCheckListComponent implements OnInit {
  constructor(
    private readonly snackbarService: SnackbarService,
    private readonly dialogService: DialogService,
    private readonly offersApiService: OffersApiService
  ) {}

  model: OfferRuleDto[] = [];
  displayedColumns: string[] = [
    'created',
    'deadline',
    'offerNumber',
    'task',
    'report',
    'completed',
    'approveBtn',
    'rejectBtn',
  ];
  dataSource!: MatTableDataSource<OfferRuleDto>;
  resultsLength: number = 0;
  isLoading = true;

  @ViewChild(MatPaginator) paginator!: MatPaginator;

  private searchText$ = new Subject<string>();
  ngOnInit(): void {
    this.offersApiService
      .apiOffersRulesChecksListGet()
      .subscribe((response) => {
        this.model = response;
        console.log(response);

        this.dataSource = new MatTableDataSource<OfferRuleDto>(response);
        this.dataSource.paginator = this.paginator;
        this.isLoading = false;
      });
  }

  approve(rule: OfferRuleDto) {
    if (!rule.id || !rule.completed) return;

    this.offersApiService
      .apiOffersRulesChecksApprovePut(rule.id)
      .subscribe((response) => {
        this.snackbarService.show({
          duration: 2000,
          message: 'Выполнение задачи принято',
        });
      });
  }

  reject(rule: OfferRuleDto) {}
}
