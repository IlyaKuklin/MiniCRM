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

  errorStateMatcher = new ManagerErrorStateMatcher();

  ngOnInit(): void {
    this.route.params.subscribe((params) => {
      if (params.id) {
        this.isEdit = true;
      }
    });
  }

  submit(): void {
    if (!this.managerForm.valid) return;
    this.isLoading = true;

    this.authApiService.apiAuthRegisterPost({
      login: this.model.login,
      name: this.model.name,
      password: this.password,
    }).subscribe(response => {
      this.isLoading = false;
      alert('Менеджер создан');
      this.router.navigate([`/managers/edit/${response.id}`]);
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
