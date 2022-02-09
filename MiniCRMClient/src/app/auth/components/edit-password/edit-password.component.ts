import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute } from '@angular/router';
import { catchError, exhaustMap, filter, switchMap, tap } from 'rxjs/operators';
import { AuthApiService, UserNewPasswordDto } from 'src/api/rest/api';
import { DialogService } from 'src/app/shared/services/dialog.service';
import { SnackbarService } from 'src/app/shared/services/snackbar.service';

@Component({
  selector: 'mcrm-edit-password',
  templateUrl: './edit-password.component.html',
  styleUrls: ['./edit-password.component.scss'],
})
export class EditPasswordComponent implements OnInit {
  constructor(
    private readonly route: ActivatedRoute,
    private readonly fb: FormBuilder,
    private readonly authApiService: AuthApiService,
    private readonly dialogService: DialogService,
    private readonly snackbarService: SnackbarService
  ) {}

  form!: FormGroup;

  id!: number;

  isLoading: boolean = false;

  get formControls() {
    return this.form['controls'];
  }

  get buttonDisabled(): boolean {
    return this.form.invalid || this.isLoading;
  }

  ngOnInit(): void {
    this.route.params.subscribe((params) => {
      this.id = params.id;
    });

    this.form = this.fb.group({
      password: ['', Validators.required],
      newPassword: ['', Validators.required],
      adminPassword: ['', Validators.required],
    });
  }

  update(): void {
    const dto: UserNewPasswordDto = {
      password: this.form.get('password')?.value,
      passwordConfirm: this.form.get('newPassword')?.value,
      adminPassword: this.form.get('adminPassword')?.value,
      managerId: this.id,
    };

    this.dialogService
      .confirmDialog({
        header: 'Обновление пароля',
        message: 'Подтвердите действие',
      })
      .pipe(
        filter((x) => !!x),
        tap(() => (this.isLoading = true)),
        exhaustMap(() =>
          this.authApiService.apiAuthManagerChangePasswordPatch(dto)
        )
      )
      .subscribe(
        (response) => {
          this.isLoading = false;
          this.snackbarService.show({
            duration: 2000,
            message: 'Пароль успешно обновлен',
          });
          this.form.reset();
        },
        () => {
          this.isLoading = false;
        }
      );
  }
}
