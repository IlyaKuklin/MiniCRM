import { DataSource } from '@angular/cdk/collections';
import { Component, OnInit, ViewChild } from '@angular/core';
import { FormControl } from '@angular/forms';
import { MatCheckboxChange } from '@angular/material/checkbox';
import { MatPaginator } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';
import { Observable, ReplaySubject, Subject } from 'rxjs';
import { switchMap } from 'rxjs/operators';
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
  dataToDisplay!: MatTableDataSource<OfferRuleDto>;
  resultsLength: number = 0;
  isLoading = true;

  @ViewChild(MatPaginator) paginator!: MatPaginator;
  @ViewChild(MatSort) sort!: MatSort;

  statusFilter = new FormControl('');
  filterValues = {
    status: '',
  };

  //private searchText$ = new Subject<string>();

  get now(): Date {
    const now = new Date();
    now.setHours(0, 0, 0, 0);
    return now;
  }

  ngOnInit(): void {
    this.offersApiService
      .apiOffersRulesChecksListGet()
      .subscribe((response) => {
        this.model = response;

        this.initTable(response);
        console.log(response);
        this.fieldListener();

        this.isLoading = false;
      });
  }

  approve(rule: OfferRuleDto) {
    if (!rule.id || !rule.completed) return;

    this.isLoading = true;

    this.offersApiService
      .apiOffersRulesChecksApprovePut(rule.id)
      .subscribe((response) => {
        this.snackbarService.show({
          duration: 2000,
          message: 'Выполнение задачи принято',
        });
        this.model = this.model.filter((x) => x.id != rule.id);
        this.initTable(this.model);
        this.isLoading = false;
      });
  }

  reject(rule: OfferRuleDto) {
    this.dialogService
      .inputDialog({
        header: 'Укажите причину отклонения',
        text: '',
      })
      .pipe(
        switchMap((result) =>
          this.offersApiService.apiOffersRulesChecksRejectPut({
            id: rule.id,
            remarks: result.text,
          })
        )
      )
      .subscribe((response) => {
        this.model = this.model.filter((x) => x.id != rule.id);
        this.initTable(this.model);
        this.snackbarService.show({
          duration: 2000,
          message: 'Выполнение задачи отклонено',
        });
      });
  }

  getDatePassed(date: Date): boolean {
    return new Date(date) < this.now;
  }

  private initTable(response: OfferRuleDto[]) {
    this.dataSource = new MatTableDataSource<OfferRuleDto>(response);
    this.dataSource.paginator = this.paginator;
    this.dataSource.filterPredicate = this.createFilter();
    this.dataSource.sort = this.sort;
  }

  private fieldListener() {
    this.statusFilter.valueChanges.subscribe((status) => {
      this.filterValues.status = status;
      this.dataSource.filter = JSON.stringify(this.filterValues);
    });
  }

  private createFilter(): (rule: OfferRuleDto, filter: string) => boolean {
    let filterFunction = function (
      rule: OfferRuleDto,
      filter: string
    ): boolean {
      let searchTerms = JSON.parse(filter);
      if (searchTerms.status) return rule.completed == searchTerms.status;
      return true;
    };

    return filterFunction;
  }
}
