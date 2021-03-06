import { MatPaginatorIntl } from '@angular/material/paginator';

const russianRangeLabel = (page: number, pageSize: number, length: number) => {
  if (length == 0 || pageSize == 0) {
    return `0 из ${length}`;
  }

  length = Math.max(length, 0);

  const startIndex = page * pageSize;

  const endIndex =
    startIndex < length
      ? Math.min(startIndex + pageSize, length)
      : startIndex + pageSize;

  return `${startIndex + 1} - ${endIndex} из ${length}`;
};

export function getRussianPaginatorIntl() {
  const paginatorIntl = new MatPaginatorIntl();

  paginatorIntl.itemsPerPageLabel = 'Вывести записей:';
  paginatorIntl.nextPageLabel = 'Следующая';
  paginatorIntl.previousPageLabel = 'Предыдущая';
  paginatorIntl.lastPageLabel = 'В конец';
  paginatorIntl.firstPageLabel = 'В начало';
  paginatorIntl.getRangeLabel = russianRangeLabel;

  return paginatorIntl;
}
