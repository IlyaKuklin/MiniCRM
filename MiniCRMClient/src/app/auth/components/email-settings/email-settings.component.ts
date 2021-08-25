import { Component, OnInit } from '@angular/core';
import { CommonApiService, EmailSettingsDto } from 'src/api/rest/api';
import { ManagerErrorStateMatcher } from '../edit-manager/edit-manager.component';

@Component({
  selector: 'mcrm-email-settings',
  templateUrl: './email-settings.component.html',
  styleUrls: ['./email-settings.component.scss'],
})
export class EmailSettingsComponent implements OnInit {
  constructor(private readonly commonApiService: CommonApiService) {}

  isLoading: boolean = false;
  model: EmailSettingsDto = {};

  errorStateMatcher = new ManagerErrorStateMatcher();

  ngOnInit(): void {
    this.isLoading = true;
    this.commonApiService
      .apiCommonSettingsGetGet()
      .subscribe((response: EmailSettingsDto) => {
        this.model = response;
        this.isLoading = false;
      });
  }

  submit() {
    this.isLoading = true;
    this.commonApiService
      .apiCommonSettingsUpdatePost(this.model)
      .subscribe(() => {
        this.isLoading = false;
      });
  }
}
