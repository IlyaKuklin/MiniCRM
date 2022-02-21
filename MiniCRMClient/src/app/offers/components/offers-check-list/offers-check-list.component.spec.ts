import { ComponentFixture, TestBed } from '@angular/core/testing';

import { OffersCheckListComponent } from './offers-check-list.component';

describe('OffersCheckListComponent', () => {
  let component: OffersCheckListComponent;
  let fixture: ComponentFixture<OffersCheckListComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ OffersCheckListComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(OffersCheckListComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
