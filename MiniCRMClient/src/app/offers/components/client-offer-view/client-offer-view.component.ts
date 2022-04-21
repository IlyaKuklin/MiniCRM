import { Component, OnInit } from '@angular/core';
import {
  OfferClientViewDto,
  OfferFeedbackRequestDto,
  OffersApiService,
  SectionDto,
} from 'src/api/rest/api';
import { isDevMode } from '@angular/core';
import { jsPDF } from 'jspdf';
import html2canvas from 'html2canvas';
import { ActivatedRoute } from '@angular/router';
import { DialogService } from 'src/app/shared/services/dialog.service';
import { SnackbarService } from 'src/app/shared/services/snackbar.service';
import { DomSanitizer } from '@angular/platform-browser';
import { OffersService } from '../../services/offers.service';

@Component({
  selector: 'mcrm-client-offer-view',
  templateUrl: './client-offer-view.component.html',
  styleUrls: ['./client-offer-view.component.scss'],
})
export class ClientOfferViewComponent implements OnInit {
  constructor(
    private readonly offersApiService: OffersApiService,
    private readonly route: ActivatedRoute,
    private readonly snackbarService: SnackbarService,
    private readonly dialogService: DialogService,
    private readonly offersService: OffersService,
    private sanitizer: DomSanitizer
  ) {}

  model!: OfferClientViewDto;
  isLoading: boolean = false;

  // TODO: to pipe (https://stackoverflow.com/questions/39857858/angular-2-domsanitizer-bypasssecuritytrusthtml-svg)
  get offerPoint() {
    var s = this.model.sections.find((x) => x.type == 'offerPoint');

    if (s) {
      var h = this.sanitizer.bypassSecurityTrustHtml(<string>s.data);
      return h;
    }
    return '';
  }

  ngOnInit(): void {
    this.route.params.subscribe((params) => {
      this.isLoading = true;

      this.offersApiService
        .apiOffersClientOfferGet(params.clientOfferId, params.key)
        .subscribe((response: OfferClientViewDto) => {
          if (isDevMode()) {
            response.sections.forEach((x) => {
              if (x.type === 'img' || x.type === 'description') {
                x.imagePaths = x.imagePaths.map(
                  (p) => `http:\\\\localhost:5000\\${p}`
                );
              }
            });
          }

          this.model = response;
          this.isLoading = false;

          this.model.sections = this.offersService.sortByName(
            this.model.sections
          );

          console.log(this.model.sections);
        });
    });
  }

  get managerEmailLink(): string {
    return `mailto:${this.model.managerEmail}`;
  }

  download() {
    this.isLoading = true;

    const btn = <HTMLElement>document.getElementById('download_btn');
    btn.style.display = 'none';
    const feedbackRequests = <HTMLElement>(
      document.getElementById('feedbackRequests')
    );
    if (feedbackRequests) feedbackRequests.style.display = 'none';

    var data = document.getElementById('offer'); //Id of the table
    if (data) {
      html2canvas(data, {
        useCORS: true,
      }).then((canvas) => {
        // Few necessary setting options
        let imgWidth = 210;
        let pageHeight = 295;
        let imgHeight = (canvas.height * imgWidth) / canvas.width;
        let heightLeft = imgHeight;

        const contentDataURL = canvas.toDataURL('image/png');
        let pdf = new jsPDF('p', 'mm', 'a4'); // A4 size page of PDF
        let position = 0;
        pdf.addImage(contentDataURL, 'PNG', 0, position, imgWidth, imgHeight);
        pdf.save(`Коммерческое предложение №${this.model.number}.pdf`); // Generated PDF

        btn.style.display = 'block';

        if (feedbackRequests) feedbackRequests.style.display = 'block';
        this.isLoading = false;
      });
    }
  }

  answerOnRequest(request: OfferFeedbackRequestDto) {
    this.offersApiService
      .apiOffersClientOfferAnswerOnFeedbackRequestPost({
        answerText: request.answerText,
        id: request.id,
      })
      .subscribe((response) => {
        this.snackbarService.show({
          message: 'Ответ отправлен',
          duration: 2000,
        });
        this.model.feedbackRequests = this.model.feedbackRequests.filter(
          (x) => x.id !== request.id
        );
      });
  }

  getIconPathByName(name: string): string {
    return this.offersService.getPathByName(name);
  }
}
