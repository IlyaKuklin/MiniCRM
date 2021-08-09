import { ComponentFixture, TestBed } from '@angular/core/testing';

import { EditCommunicationReportComponent } from './edit-communication-report.component';

describe('EditCommunicationReportComponent', () => {
  let component: EditCommunicationReportComponent;
  let fixture: ComponentFixture<EditCommunicationReportComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ EditCommunicationReportComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(EditCommunicationReportComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
