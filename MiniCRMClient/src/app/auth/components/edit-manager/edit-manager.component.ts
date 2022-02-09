import { Component, Input, OnInit, ViewChild } from '@angular/core';
import { FormControl, FormGroupDirective, NgForm } from '@angular/forms';
import { ErrorStateMatcher } from '@angular/material/core';
import { ActivatedRoute, Router } from '@angular/router';
import { of } from 'rxjs';
import {
  exhaustMap,
  filter,
  map,
  mergeMap,
  switchMap,
  tap,
} from 'rxjs/operators';
import { AuthApiService, UserDto } from 'src/api/rest/api';
import { DialogService } from 'src/app/shared/services/dialog.service';
import { SnackbarService } from 'src/app/shared/services/snackbar.service';

@Component({
  selector: 'mcrm-edit-manager',
  templateUrl: './edit-manager.component.html',
  styleUrls: ['./edit-manager.component.scss'],
})
export class EditManagerComponent implements OnInit {
  constructor(
    private readonly route: ActivatedRoute,
    private readonly router: Router,
    private readonly authApiService: AuthApiService,
    private readonly dialogService: DialogService,
    private readonly snackbarService: SnackbarService
  ) {}

  @ViewChild('managerForm') managerForm!: NgForm;
  isLoading: boolean = false;

  model: UserDto = {};
  password: string = '';
  isEdit: boolean = false;

  private originalName: string = '';
  private originalLogin: string = '';
  private originalEmail: string = '';

  errorStateMatcher = new ManagerErrorStateMatcher();

  ngOnInit(): void {
    this.route.params.subscribe((params) => {
      if (params.id) {
        this.isEdit = true;
        this.isLoading = true;

        this.authApiService
          .apiAuthManagerGet(params.id)
          .subscribe((response) => {
            this.model = response;
            this.originalLogin = <string>response.login;
            this.originalName = <string>response.name;
            this.originalEmail = <string>response.email;
            this.isLoading = false;
          });
      }
    });
  }

  get modelChanged(): boolean {
    return (
      this.isEdit &&
      (this.originalLogin !== this.model.login ||
        this.originalName !== this.model.name ||
        this.originalEmail !== this.model.email)
    );
  }

  submit(): void {
    if (!this.managerForm.valid) return;
    this.isLoading = true;

    this.authApiService
      .apiAuthRegisterPost({
        login: this.model.login,
        name: this.model.name,
        email: this.model.email,
        password: this.password,
      })
      .subscribe((response) => {
        this.isLoading = false;
        alert('Менеджер создан');
        this.router.navigate([`/managers/edit/${response.id}`]);
      });
  }

  update(): void {
    this.isLoading = true;
    this.authApiService
      .apiAuthManagerUpdatePatch(this.model)
      .subscribe((response) => {
        this.model = response;
        this.originalName = <string>response.name;
        this.originalLogin = <string>response.login;
        this.originalEmail = <string>response.email;
        alert('Данные обновлены');
        this.isLoading = false;
      });
  }

  delete(): void {
    this.dialogService
      .confirmDialog({
        header: 'Удаление',
        message: 'Вы уверены, что хотите удалить менеджера?',
      })
      .pipe(
        filter((x) => !!x),
        tap(() => (this.isLoading = true)),
        switchMap(() =>
          this.authApiService.apiAuthManagerDeleteDelete(this.model.id)
        )
      )
      .subscribe((response) => {
        this.snackbarService.show({
          message: 'Менеджер удалён',
          duration: 2000,
        });
        this.isLoading = false;
        this.router.navigate(['/managers']);
      });
  }
}

export class ManagerErrorStateMatcher implements ErrorStateMatcher {
  isErrorState(
    control: FormControl | null,
    form: FormGroupDirective | NgForm | null
  ): boolean {
    const isSubmitted = form && form.submitted;
    return !!(control && control.invalid && (control.touched || isSubmitted));
  }
}
