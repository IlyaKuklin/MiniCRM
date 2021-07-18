import { HttpErrorResponse } from '@angular/common/http';
import { Component, OnInit, ViewChild } from '@angular/core';
import { FormControl, FormGroupDirective, NgForm } from '@angular/forms';
import { ErrorStateMatcher } from '@angular/material/core';
import { Router } from '@angular/router';
import { AuthApiService, UserAuthDto } from 'src/api/rest/api';
import { AuthService } from '../../services/auth.service';

@Component({
  selector: 'mcrm-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.scss'],
})
export class LoginComponent implements OnInit {
  constructor(
    private readonly authApiService: AuthApiService,
    private readonly authService: AuthService,
    private readonly router: Router
  ) {}

  @ViewChild('loginForm') loginForm!: NgForm;

  errorStateMatcher = new LoginErrorStateMatcher();

  loginModel: UserAuthDto = {
    login: '',
    password: '',
  };

  isLoading: boolean = false;

  ngOnInit(): void {}

  loginClick(): void {
    if (!this.loginForm.valid) return;
    this.isLoading = true;
    this.authApiService.apiAuthLoginPost(this.loginModel).subscribe(
      (response) => {
        this.isLoading = false;
        this.authService.setUserData(response);
        this.router.navigate(['/']);
      },
      (error: HttpErrorResponse) => {
        this.isLoading = false;
        if (error.error.Message) {
          alert(error.error.Message);
        } else alert(error.message);
      }
    );
  }
}

export class LoginErrorStateMatcher implements ErrorStateMatcher {
  isErrorState(
    control: FormControl | null,
    form: FormGroupDirective | NgForm | null
  ): boolean {
    const isSubmitted = form && form.submitted;
    return !!(control && control.invalid && (control.touched || isSubmitted));
  }
}
