import { ComponentFixture, TestBed } from '@angular/core/testing';

import { CommunicationReportsListComponent } from './communication-reports-list.component';

describe('CommunicationReportsListComponent', () => {
  let component: CommunicationReportsListComponent;
  let fixture: ComponentFixture<CommunicationReportsListComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ CommunicationReportsListComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(CommunicationReportsListComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
