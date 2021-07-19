import { Component, OnInit, ViewChild } from '@angular/core';
import { MatPaginator } from '@angular/material/paginator';
import { MatTableDataSource } from '@angular/material/table';
import { AuthApiService, UserDto } from 'src/api/rest/api';

@Component({
  selector: 'mcrm-managers-list',
  templateUrl: './managers-list.component.html',
  styleUrls: ['./managers-list.component.scss'],
})
export class ManagersListComponent implements OnInit {
  constructor(
    private readonly authApiService: AuthApiService
  ) {}

  model: UserDto[] = [];
  displayedColumns: string[] = ['id', 'name', 'login', 'del'];
  dataSource!: MatTableDataSource<UserDto>;
  resultsLength: number = 0;
  isLoading = true;

  @ViewChild(MatPaginator) paginator!: MatPaginator;

  ngOnInit(): void {
    this.authApiService.apiAuthManagersGet().subscribe((response) => {
      this.model = response;
      this.dataSource = new MatTableDataSource<UserDto>(response);
      this.dataSource.paginator = this.paginator;
      this.isLoading = false;
    });
  }

  delete(id: number) {
    this.isLoading = true;
    this.authApiService.apiAuthManagerDeleteDelete(id).subscribe((response) => {
      this.model = this.model.filter((x) => x.id !== id);
      this.dataSource = new MatTableDataSource<UserDto>(this.model);
      this.isLoading = false;
    });
  }
}
