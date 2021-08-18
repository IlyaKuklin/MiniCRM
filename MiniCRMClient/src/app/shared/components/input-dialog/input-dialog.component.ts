import { Component, Inject, OnInit } from '@angular/core';
import { MAT_DIALOG_DATA } from '@angular/material/dialog';
import { IInputDialogData } from '../../services/dialog.service';

@Component({
  selector: 'mcrm-input-dialog',
  templateUrl: './input-dialog.component.html',
  styleUrls: ['./input-dialog.component.scss'],
})
export class InputDialogComponent implements OnInit {
  constructor(@Inject(MAT_DIALOG_DATA) public data: IInputDialogData) {}

  ngOnInit(): void {}
}
