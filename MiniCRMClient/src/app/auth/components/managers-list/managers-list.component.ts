import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';

@Component({
  selector: 'mcrm-managers-list',
  templateUrl: './managers-list.component.html',
  styleUrls: ['./managers-list.component.scss']
})
export class ManagersListComponent implements OnInit {

  constructor(private readonly router: Router) { }

  ngOnInit(): void {
  }


}
