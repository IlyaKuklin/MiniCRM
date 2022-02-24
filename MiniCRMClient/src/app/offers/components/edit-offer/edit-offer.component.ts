import { Component, OnInit, ViewChild } from '@angular/core';
import { NgForm, FormControl, FormGroupDirective } from '@angular/forms';
import { MatCheckboxChange } from '@angular/material/checkbox';
import { ErrorStateMatcher } from '@angular/material/core';
import { ActivatedRoute, Router } from '@angular/router';
import { forkJoin, pipe } from 'rxjs';
import {
  ClientDto,
  ClientsApiService,
  OfferCheckStatus,
  OfferDto,
  OfferFeedbackRequestDto,
  OfferFileDatumDto,
  OfferFileType,
  OfferNewsbreakDto,
  OfferRuleDto,
  OffersApiService,
} from 'src/api/rest/api';
import { DialogService } from 'src/app/shared/services/dialog.service';
import { SnackbarService } from 'src/app/shared/services/snackbar.service';
import { isDevMode } from '@angular/core';
import { Observable } from 'rxjs';
import { filter, map, startWith } from 'rxjs/operators';

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
    private readonly router: Router,
    private readonly snackbarService: SnackbarService,
    private readonly dialogService: DialogService
  ) {}

  @ViewChild('offerForm') offerForm!: NgForm;
  isLoading: boolean = false;

  model: OfferDto = {
    selectedSections: [],
    rules: [],
    commonCommunicationReports: [],
  };
  isEdit: boolean = false;
  originalModel: OfferDto = {
    selectedSections: [],
    rules: [],
    commonCommunicationReports: [],
  };
  clients: ClientDto[] = [];

  errorStateMatcher = new OfferErrorStateMatcher();

  clientSelectControl = new FormControl();
  filteredClients!: Observable<ClientDto[]>;

  potentialList: string[] = ['Холодный', 'Тёплый', 'Горячий'];

  get modelChanged(): boolean {
    return true;
  }

  get clientSelected() {
    return typeof this.clientSelectControl.value == 'object';
  }

  get canCreate() {
    return (
      this.clientSelectControl.value &&
      typeof this.clientSelectControl.value == 'object'
    );
  }

  ngOnInit(): void {
    console.log(isDevMode());

    this.filteredClients = this.clientSelectControl.valueChanges.pipe(
      startWith(''),
      map((value) => this._filter(value))
    );

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

          // TODO: remove
          if (this.model.fileData && isDevMode()) {
            this.model.fileData.forEach((x) => {
              x.path = `http:\\\\localhost:5000\\${x.path}`;
            });
          }

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
    if (this.model.clientId == undefined) {
      alert('Не выбран клиент');
      return;
    }
    this.isLoading = true;

    this.offersApiService
      .apiOffersEditPost(false, this.model)
      .subscribe((response) => {
        this.isLoading = false;
        this.snackbarService.show({
          message: 'КП создано',
          duration: 3000,
        });
        this.router.navigate([`/offers/edit/${response.id}`]);
      });
  }

  update(): void {
    this.isLoading = true;
    this.offersApiService
      .apiOffersEditPost(false, this.model)
      .subscribe((response) => {
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
        header: 'Удаление',
        message: 'Вы уверены, что хотите удалить КП?',
      })
      .subscribe((result) => {
        if (result) {
          this.isLoading = true;
          this.offersApiService
            .apiOffersDeleteDelete(this.model.id)
            .subscribe((response) => {
              this.snackbarService.show({
                message: 'КП удалёно',
                duration: 3000,
              });
              this.isLoading = false;
              this.router.navigate(['/offers']);
            });
        }
      });
  }

  onFieldSelectChange(evt: MatCheckboxChange) {
    if (evt.checked) this.model.selectedSections.push(<string>evt.source.name);
    else
      this.model.selectedSections = this.model.selectedSections.filter(
        (x) => x !== <string>evt.source.name
      );
    console.log(this.model.selectedSections);
  }

  isFieldSelected(name: string) {
    return this.model.selectedSections.indexOf(name) > -1;
  }

  //#region Files

  getFiles(type: OfferFileType): OfferFileDatumDto[] {
    if (this.model.fileData) {
      return this.model.fileData.filter((x) => x.type == type);
    }
    return [];
  }

  uploadFile(
    files: FileList | null,
    type: OfferFileType,
    replace: boolean
  ): void {
    if (files === null || files.length === 0) {
      return;
    }

    const blobs = [];
    for (var i = 0; i < files.length; i++) {
      const file = files[i];
      blobs.push(<Blob>file);
    }

    this.isLoading = true;
    this.offersApiService
      .apiOffersFilesUploadPatch(<number>this.model.id, type, replace, blobs)
      .subscribe((response: OfferFileDatumDto[]) => {
        if (replace) {
          this.model.fileData = this.model.fileData?.filter(
            (x) => x.type != type
          );
        }
        response.forEach((x) => {
          this.model.fileData?.push(x);
        });

        this.isLoading = false;
      });
  }

  deleteFile(fileId: number | undefined): void {
    if (!fileId) return;

    this.dialogService
      .confirmDialog({
        header: 'Удаление',
        message: 'Вы уверены, что хотите удалить файл?',
      })
      .subscribe((result) => {
        if (result) {
          this.isLoading = true;
          this.offersApiService
            .apiOffersFilesDeleteDelete(fileId)
            .subscribe(() => {
              this.model.fileData = this.model.fileData?.filter(
                (x) => x.id != fileId
              );
              this.isLoading = false;
            });
        }
      });
  }

  //#endregion

  onClientSelected(evt: ClientDto) {
    this.model.clientId = evt.id;
  }

  getClientOptionText(option: ClientDto): string {
    return option?.name ? option.name : '';
  }

  sendClick(): void {
    if (!this.model.email) {
      this.snackbarService.show({
        message: 'Укажите email для отправки письма',
        duration: 2000,
        isError: true,
      });
      return;
    }

    this.dialogService
      .confirmDialog({
        header: 'Отправка КП',
        message: `Вы уверены, что хотите отправить текущую версию КП клиенту на адрес электронной почты ${this.model.email}?`,
      })
      .subscribe((result) => {
        if (result) {
          this.isLoading = true;
          this.offersApiService
            .apiOffersEditPost(true, this.model)
            .subscribe((response) => {
              this.offersApiService
                .apiOffersClientOfferSendPost(this.model.id)
                .subscribe(
                  (response: string) => {
                    this.snackbarService.show({
                      message: 'КП отправлено клиенту',
                      duration: 2000,
                    });

                    this.dialogService.infoDialog({
                      header: '',
                      message: response,
                    });

                    this.isLoading = false;
                  },
                  (e) => {
                    console.log(e.error.Message);
                    alert(e.error.Message);
                  }
                );
            });
        }
      });
  }

  onAddNewsbreakClick(): void {
    this.dialogService
      .inputDialog({
        header: 'Добавление инфоповода',
        text: '',
      })
      .subscribe((result: { text: string }) => {
        if (result && result.text.length > 0) {
          this.isLoading = true;

          this.offersApiService
            .apiOffersNewsbreaksAddPost({
              offerId: this.model.id,
              text: result.text,
            })
            .subscribe((response: OfferNewsbreakDto) => {
              this.model.newsbreaks?.push(response);
              this.isLoading = false;
            });
        }
      });
  }

  //#region FeedbackRequests
  onAddFeedbackRequestClick(): void {
    this.dialogService
      .inputDialog({
        header: 'Добавление заявки на обратную связь',
        text: '',
      })
      .subscribe((result: { text: string }) => {
        if (result && result.text.length > 0) {
          this.isLoading = true;

          this.offersApiService
            .apiOffersFeedbackRequestsAddPost({
              offerId: this.model.id,
              text: result.text,
            })
            .subscribe((response: OfferFeedbackRequestDto) => {
              this.model.feedbackRequests?.push(response);
              this.isLoading = false;
            });
        }
      });
  }

  showAnswer(request: OfferFeedbackRequestDto) {
    this.dialogService.infoDialog({
      header: '',
      message: <string>request.answerText,
    });
  }
  //#endregion

  //#region Rules

  onAddRuleClick(): void {
    this.model.rules.push({
      completed: false,
      offerId: this.model.id,
      id: 0,
    });
  }

  cancelRuleAdd(rule: OfferRuleDto): void {
    this.model.rules = this.model.rules.filter((x) => x != rule);
  }

  confirmRuleAdd(rule: OfferRuleDto): void {
    if (rule.completed) return;

    if (!rule.task) {
      this.snackbarService.show({
        duration: 2000,
        message: 'Заполните текст задачи',
        isError: true,
      });
      return;
    } else if (!rule.deadline) {
      this.snackbarService.show({
        duration: 2000,
        message: 'Укажите срок исполнения',
        isError: true,
      });
      return;
    }

    this.isLoading = true;
    this.offersApiService
      .apiOffersRulesEditPost(rule)
      .subscribe((response: OfferRuleDto) => {
        rule.id = response.id;
        this.isLoading = false;
      });
  }

  completeRule(rule: OfferRuleDto, $event: any): void {
    if (rule.completed) return;
    $event.preventDefault();

    if (!rule.report?.length) {
      this.snackbarService.show({
        message: 'Заполните отчёт',
        duration: 2000,
        isError: true,
      });
      return;
    }

    this.dialogService
      .confirmDialog({
        header: 'Выполнение задачи',
        message: 'Подтвердите выполнение задачи',
      })
      .subscribe((result) => {
        if (result) {
          this.offersApiService
            .apiOffersRulesCompletePost({
              id: rule.id,
              report: rule.report,
            })
            .subscribe((response) => {
              this.snackbarService.show({
                duration: 2000,
                message: 'Задача помечена выполненной',
              });
              rule.completed = true;
            });
        }
      });
  }

  deleteRule(ruleId: number): void {
    const rule = this.model.rules.find((x) => x.id == ruleId);
    if (rule?.completed) return;
    this.dialogService
      .confirmDialog({
        header: 'Удаление правила',
        message: 'Вы уверены, что хотите удалить правило?',
      })
      .subscribe((response) => {
        if (response) {
          this.isLoading = true;
          this.offersApiService
            .apiOffersRulesDeleteDelete(ruleId)
            .subscribe(() => {
              this.model.rules = this.model.rules.filter(
                (x) => x.id !== ruleId
              );
              this.snackbarService.show({
                duration: 2000,
                message: 'Правило удалено',
              });
              this.isLoading = false;
            });
        }
      });
  }

  getRuleCheckStatus(ruleCheckStatus: OfferCheckStatus): string {
    if (ruleCheckStatus == OfferCheckStatus.NUMBER_1) return 'Принято';
    if (ruleCheckStatus == OfferCheckStatus.NUMBER_2) return 'Не принято';
    return '';
  }

  showRuleRejectionReason(rule: OfferRuleDto): void {
    if (!rule.checkRemark) return;

    this.dialogService.infoDialog({
      message: rule.checkRemark,
      header: 'Причина отклонения',
    });
  }
  //#endregion

  private _filter(value: any): ClientDto[] {
    const isString = (value as ClientDto).id == undefined;
    if (!isString) return [];

    const filterValue = value.toLowerCase();
    return this.clients.filter((option) => {
      const name = <string>option.name;
      return name.toLowerCase().includes(filterValue);
    });
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
