import { Component, Inject, OnInit } from '@angular/core';
import { MAT_DIALOG_DATA } from '@angular/material/dialog';
import { IInfoDialogData } from '../../services/dialog.service';

@Component({
  selector: 'sm-info-dialog',
  templateUrl: './info-dialog.component.html',
  styleUrls: ['./info-dialog.component.scss'],
})
export class InfoDialogComponent implements OnInit {
  constructor(@Inject(MAT_DIALOG_DATA) public data: IInfoDialogData) {}

  ngOnInit(): void {}
}
