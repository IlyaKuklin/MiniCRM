import { Injectable } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { Observable } from 'rxjs';
import { ConfirmDialogComponent } from '../components/confirm-dialog/confirm-dialog.component';
import { InfoDialogComponent } from '../components/info-dialog/info-dialog.component';
import { SelectOptionDialogComponent } from '../components/select-option-dialog/select-option-dialog.component';

@Injectable({
  providedIn: 'root',
})
export class DialogService {
  constructor(public dialog: MatDialog) {}
  selectOptionDialog(data: ISelectOptionDialogData): Observable<any> {
    const dialogRef = this.dialog.open(SelectOptionDialogComponent, {
      data: data,
    });
    return dialogRef.afterClosed();
  }

  confirmDialog(data: IConfirmDialogData): Observable<any> {
    const dialogRef = this.dialog.open(ConfirmDialogComponent, {
      data: data,
    });
    return dialogRef.afterClosed();
  }

  infoDialog(data: IInfoDialogData): Observable<any> {
    const dialogRef = this.dialog.open(InfoDialogComponent, {
      data: data,
      panelClass: data.isError ? 'errorDialog' : '',
    });
    return dialogRef.afterClosed();
  }
}

export interface ISelectOptionDialogData {
  header: string;
  options: string[];
}

export interface IConfirmDialogData {
  header: string;
  message: string;
}

export interface IInfoDialogData {
  header: string;
  message: string;
  additionalData?: string;
  isError?: boolean;
}
