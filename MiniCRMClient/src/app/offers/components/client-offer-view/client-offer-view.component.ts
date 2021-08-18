import { Component, OnInit } from '@angular/core';
import { OfferClientViewDto, OffersApiService } from 'src/api/rest/api';
import { isDevMode } from '@angular/core';

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
          this.model = response;
        }
      });
  }
}
