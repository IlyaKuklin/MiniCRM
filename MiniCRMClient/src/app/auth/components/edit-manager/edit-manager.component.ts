import { Component, Input, OnInit, ViewChild } from '@angular/core';
import { FormControl, FormGroupDirective, NgForm } from '@angular/forms';
import { ErrorStateMatcher } from '@angular/material/core';
import { ActivatedRoute, Router } from '@angular/router';
import { AuthApiService, UserDto } from 'src/api/rest/api';

@Component({
  selector: 'mcrm-edit-manager',
  templateUrl: './edit-manager.component.html',
  styleUrls: ['./edit-manager.component.scss'],
})
export class EditManagerComponent implements OnInit {
  constructor(
    private readonly route: ActivatedRoute,
    private readonly authApiService: AuthApiService,
    private readonly router: Router
  ) {}

  @ViewChild('managerForm') managerForm!: NgForm;
  isLoading: boolean = false;

  model: UserDto = {};
  password: string = '';
  isEdit: boolean = false;

  private originalName: string = '';
  private originalLogin: string = '';

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
            this.isLoading = false;
          });
      }
    });
  }

  get modelChanged(): boolean {
    return (
      this.isEdit &&
      (this.originalLogin !== this.model.login ||
        this.originalName !== this.model.name)
    );
  }

  submit(): void {
    if (!this.managerForm.valid) return;
    this.isLoading = true;

    this.authApiService
      .apiAuthRegisterPost({
        login: this.model.login,
        name: this.model.name,
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
        alert('Данные обновлены');
        this.isLoading = false;
      });
  }

  delete(): void {
    this.isLoading = true;
    this.authApiService
      .apiAuthManagerDeleteDelete(this.model.id)
      .subscribe((response) => {
        alert('Менеджер удалён');
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
