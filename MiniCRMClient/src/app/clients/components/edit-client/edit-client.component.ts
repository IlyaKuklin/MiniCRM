import { Component, OnInit, ViewChild } from '@angular/core';
import { NgForm, FormControl, FormGroupDirective } from '@angular/forms';
import { ErrorStateMatcher } from '@angular/material/core';
import { ActivatedRoute, Router } from '@angular/router';
import { ClientDto, ClientsApiService } from 'src/api/rest/api';

@Component({
  selector: 'mcrm-edit-client',
  templateUrl: './edit-client.component.html',
  styleUrls: ['./edit-client.component.scss'],
})
export class EditClientComponent implements OnInit {
  constructor(
    private readonly route: ActivatedRoute,
    private readonly clientsApiService: ClientsApiService,
    private readonly router: Router
  ) {}

  @ViewChild('clientForm') managerForm!: NgForm;
  isLoading: boolean = false;

  model: ClientDto = {};
  isEdit: boolean = false;

  originalModel: ClientDto = {};

  errorStateMatcher = new ClientErrorStateMatcher();

  ngOnInit(): void {
    this.route.params.subscribe((params) => {
      if (params.id) {
        this.isEdit = true;
        this.isLoading = true;

        this.clientsApiService
          .apiClientsGet(params.id)
          .subscribe((response) => {
            this.model = response;
            this.originalModel = { ...response };

            this.isLoading = false;
          });
      }
    });
  }

  get modelChanged(): boolean {
    return this.isEdit && this.originalModel.name != this.model.name;
  }

  submit(): void {
    if (!this.managerForm.valid) return;
    this.isLoading = true;

    this.clientsApiService
      .apiClientsEditPost(this.model)
      .subscribe((response) => {
        this.isLoading = false;
        alert('Клиент создан');
        this.router.navigate([`/clients/edit/${response.id}`]);
      });
  }

  update(): void {
    this.isLoading = true;
    this.clientsApiService
      .apiClientsEditPost(this.model)
      .subscribe((response) => {
        this.model = response;
        alert('Данные обновлены');
        this.isLoading = false;
      });
  }

  delete(): void {
    this.isLoading = true;
    this.clientsApiService
      .apiClientsDeleteDelete(this.model.id)
      .subscribe((response) => {
        alert('Клиент удалён');
        this.isLoading = false;
        this.router.navigate(['/clients']);
      });
  }
}

export class ClientErrorStateMatcher implements ErrorStateMatcher {
  isErrorState(
    control: FormControl | null,
    form: FormGroupDirective | NgForm | null
  ): boolean {
    const isSubmitted = form && form.submitted;
    return !!(control && control.invalid && (control.touched || isSubmitted));
  }
}
