import { Component, OnInit } from '@angular/core';
import { OfferClientViewDto, OffersApiService } from 'src/api/rest/api';
import { isDevMode } from '@angular/core';
import { jsPDF } from 'jspdf';
import html2canvas from 'html2canvas';

@Component({
  selector: 'mcrm-client-offer-view',
  templateUrl: './client-offer-view.component.html',
  styleUrls: ['./client-offer-view.component.scss'],
})
export class ClientOfferViewComponent implements OnInit {
  constructor(private readonly offersApiService: OffersApiService) {}

  model!: OfferClientViewDto;

  ngOnInit(): void {
    this.offersApiService
      .apiOffersClientOfferGet('00000000-0000-0000-0000-000000000000', 'key')
      .subscribe((response: OfferClientViewDto) => {
        console.log(isDevMode());

        if (isDevMode()) {
          response.sections.forEach((x) => {
            if (x.type === 'img') {
              x.imagePaths = x.imagePaths.map(
                (p) => `http:\\\\vm469442.eurodir.ru\\${p}`
              );
            }
          });
        }

        this.model = response;
      });
  }

  download() {
    var btn = <HTMLElement>document.getElementById('download_btn');
    btn.style.display = 'none';

    var data = document.getElementById('offer'); //Id of the table
    if (data) {
      html2canvas(data, {
        useCORS: true,
      }).then((canvas) => {
        // Few necessary setting options
        let imgWidth = 208;
        let pageHeight = 295;
        let imgHeight = (canvas.height * imgWidth) / canvas.width;
        let heightLeft = imgHeight;

        const contentDataURL = canvas.toDataURL('image/png');
        let pdf = new jsPDF('p', 'mm', 'a4'); // A4 size page of PDF
        let position = 0;
        pdf.addImage(contentDataURL, 'PNG', 0, position, imgWidth, imgHeight);
        pdf.save('MYPdf.pdf'); // Generated PDF
      });
    }
  }
}
