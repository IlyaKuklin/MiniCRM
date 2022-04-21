import { Injectable } from '@angular/core';
import { SectionDto } from 'src/api/rest/api';

@Injectable({
  providedIn: 'root',
})
export class OffersService {
  test = '';

  map: Map<string, IOfferSectionData> = new Map<string, IOfferSectionData>([
    [
      'Фотографии и описание товара',
      { index: 1, iconPath: 'assets/static/NewspaperClipping.svg' },
    ],
    [
      'Тип товара/системы',
      { index: 2, iconPath: 'assets/static/Bookmarks.svg' },
    ],
    [
      'Краткое описание отрасли',
      { index: 3, iconPath: 'assets/static/Newspaper.svg' },
    ],
    ['Кейс', { index: 4, iconPath: 'assets/static/Briefcase.svg' }],
    ['Суть предложения', { index: 5, iconPath: 'assets/static/Info.svg' }],
    ['Рекомендации', { index: 6, iconPath: 'assets/static/ClipboardText.svg' }],
    [
      'Технический паспорт',
      { index: 7, iconPath: 'assets/static/Notebook.svg' },
    ],
    ['Сертификат', { index: 8, iconPath: 'assets/static/Square.svg' }],
    [
      'Сопроводительное письмо',
      { index: 9, iconPath: 'assets/static/EnvelopeSimple.svg' },
    ],
    ['Аналогичные кейсы', { index: 10, iconPath: 'assets/static/Stack.svg' }],
    [
      'Прочая документация',
      { index: 11, iconPath: 'assets/static/FolderOpen.svg' },
    ],
    [
      'Визитка',
      { index: 12, iconPath: 'assets/static/IdentificationCard.svg' },
    ],
  ]);

  getPathByName(name: string): string {
    if (!this.map.has(name)) return '';
    const data = <IOfferSectionData>this.map.get(name);
    return data.iconPath;
  }

  sortByName(sections: Array<SectionDto>): Array<SectionDto> {
    return sections.sort(this.sortFn.bind(this));
  }

  private sortFn(a: SectionDto, b: SectionDto) {
    if (!this.map.has(a.name) || !this.map.has(b.name)) return 0;

    const aData = <IOfferSectionData>this.map.get(a.name);
    const bData = <IOfferSectionData>this.map.get(b.name);

    return aData.index > bData.index ? 1 : -1;
  }
}

interface IOfferSectionData {
  index: number;
  iconPath: string;
}
