import { Component, OnInit, ViewChild } from '@angular/core';
import { NgForm, FormControl, FormGroupDirective } from '@angular/forms';
import { ErrorStateMatcher } from '@angular/material/core';
import { ActivatedRoute, Router } from '@angular/router';
import { ClientDto, ClientsApiService, OfferDto } from 'src/api/rest/api';
import { SnackbarService } from 'src/app/shared/services/snackbar.service';

@Component({
  selector: 'mcrm-edit-client',
  templateUrl: './edit-client.component.html',
  styleUrls: ['./edit-client.component.scss'],
})
export class EditClientComponent implements OnInit {
  constructor(
    private readonly route: ActivatedRoute,
    private readonly clientsApiService: ClientsApiService,
    private readonly router: Router,
    private readonly snackbarService: SnackbarService
  ) {}

  @ViewChild('clientForm') managerForm!: NgForm;
  isLoading: boolean = false;

  model: ClientDto = {};
  isEdit: boolean = false;

  originalModel: ClientDto = {};

  errorStateMatcher = new ClientErrorStateMatcher();

  get modelChanged(): boolean {
    return (
      this.isEdit &&
      (this.originalModel.name != this.model.name ||
        this.originalModel.legalEntitiesNames !=
          this.model.legalEntitiesNames ||
        this.originalModel.contact != this.model.contact ||
        this.originalModel.diagnostics != this.model.diagnostics ||
        this.originalModel.domainNames != this.model.domainNames)
    );
  }

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

  submit(): void {
    if (!this.managerForm.valid) return;
    this.isLoading = true;

    this.clientsApiService
      .apiClientsEditPost(this.model)
      .subscribe((response) => {
        this.isLoading = false;
        this.snackbarService.show({
          message: 'Клиент создан',
          duration: 3000,
        });
        this.router.navigate([`/clients/edit/${response.id}`]);
      });
  }

  update(): void {
    this.isLoading = true;
    this.clientsApiService
      .apiClientsEditPost(this.model)
      .subscribe((response) => {
        this.model = response;
        this.originalModel = { ...response };
        this.snackbarService.show({
          message: 'Данные обновлены',
          duration: 3000,
        });
        this.isLoading = false;
      });
  }

  delete(): void {
    this.isLoading = true;
    this.clientsApiService
      .apiClientsDeleteDelete(this.model.id)
      .subscribe((response) => {
        this.snackbarService.show({
          message: 'Клиент удалён',
          duration: 3000,
        });
        this.isLoading = false;
        this.router.navigate(['/clients']);
      });
  }

  getOfferLink(offer: OfferDto): string {
    return `/offers/edit/${offer.id}`;
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
