import { Component, OnInit, ViewChild } from '@angular/core';
import { NgForm, FormControl, FormGroupDirective } from '@angular/forms';
import { MatCheckboxChange } from '@angular/material/checkbox';
import { ErrorStateMatcher } from '@angular/material/core';
import { ActivatedRoute, Router } from '@angular/router';
import { forkJoin, pipe } from 'rxjs';
import {
  ClientDto,
  ClientsApiService,
  OfferDto,
  OffersApiService,
} from 'src/api/rest/api';

@Component({
  selector: 'mcrm-edit-offer',
  templateUrl: './edit-offer.component.html',
  styleUrls: ['./edit-offer.component.scss'],
})
export class EditOfferComponent implements OnInit {
  constructor(
    private readonly route: ActivatedRoute,
    private readonly offersApiService: OffersApiService,
    private readonly clientsApiService: ClientsApiService,
    private readonly router: Router
  ) {}

  @ViewChild('offerForm') offerForm!: NgForm;
  isLoading: boolean = false;

  model: OfferDto = {};
  isEdit: boolean = false;
  originalModel: OfferDto = {};
  clients: ClientDto[] = [];

  errorStateMatcher = new OfferErrorStateMatcher();

  selectedFields: string[] = [];

  get modelChanged(): boolean {
    return true;
  }

  ngOnInit(): void {
    this.route.params.subscribe((params) => {
      if (params.id) {
        this.isEdit = true;
        this.isLoading = true;

        forkJoin([
          this.offersApiService.apiOffersGet(params.id),
          this.clientsApiService.apiClientsListGet(),
        ]).subscribe((response: [OfferDto, ClientDto[]]) => {
          console.log(response);

          this.model = response[0];
          this.originalModel = { ...response[0] };
          this.clients = response[1];

          this.isLoading = false;
        });
      } else {
        this.clientsApiService.apiClientsListGet().subscribe((response) => {
          this.clients = response;
        });
      }
    });
  }

  submit(): void {
    if (!this.offerForm.valid) return;
    if (this.model.clientId == undefined) {
      alert('Не выбран клиент');
      return;
    }
    this.isLoading = true;

    this.offersApiService
      .apiOffersEditPost(this.model)
      .subscribe((response) => {
        this.isLoading = false;
        alert('КП создано');
        this.router.navigate([`/offers/edit/${response.id}`]);
      });
  }

  update(): void {
    if (!this.offerForm.valid) return;
    this.isLoading = true;
    this.offersApiService
      .apiOffersEditPost(this.model)
      .subscribe((response) => {
        this.model = response;
        alert('Данные обновлены');
        this.isLoading = false;
      });
  }

  delete(): void {
    this.isLoading = true;
    this.offersApiService
      .apiOffersDeleteDelete(this.model.id)
      .subscribe((response) => {
        alert('КП удалёно');
        this.isLoading = false;
        this.router.navigate(['/offers']);
      });
  }

  onFieldSelectChange(evt: MatCheckboxChange) {
    if (evt.checked) this.selectedFields.push(<string>evt.source.name);
    else
      this.selectedFields = this.selectedFields.filter(
        (x) => x !== <string>evt.source.name
      );
    console.log(this.selectedFields);
  }

  isFieldSelected(name: string) {
    return this.selectedFields.indexOf(name) > -1;
  }
}

export class OfferErrorStateMatcher implements ErrorStateMatcher {
  isErrorState(
    control: FormControl | null,
    form: FormGroupDirective | NgForm | null
  ): boolean {
    const isSubmitted = form && form.submitted;
    return !!(control && control.invalid && (control.touched || isSubmitted));
  }
}
