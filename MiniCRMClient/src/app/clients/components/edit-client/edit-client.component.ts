import { Component, OnInit, ViewChild } from '@angular/core';
import { NgForm, FormControl, FormGroupDirective } from '@angular/forms';
import { ErrorStateMatcher } from '@angular/material/core';
import { ActivatedRoute, Router } from '@angular/router';
import {
  ClientDto,
  ClientsApiService,
  OfferDto,
} from 'src/api/rest/api';
import { DialogService } from 'src/app/shared/services/dialog.service';
import { SnackbarService } from 'src/app/shared/services/snackbar.service';
import { ClientsService } from '../../services/clients.service';

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
    private readonly snackbarService: SnackbarService,
    private readonly dialogService: DialogService
  ) {}

  @ViewChild('clientForm') clientForm!: NgForm;
  isLoading: boolean = false;

  model!: ClientDto;
  originalModel!: ClientDto;
  isEdit: boolean = false;

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
      } else {
        this.model = { id: 0, commonCommunicationReports: [] };
      }
    });
  }

  submit(): void {
    if (!this.clientForm.valid) return;
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
    if (!this.clientForm.valid) return;

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
    this.dialogService
      .confirmDialog({
        header: 'Удаление клиента',
        message: 'Вы уверены, что хотите удалить клиента?',
      })
      .subscribe((result: boolean) => {
        if (result) {
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
        } else {
        }
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
