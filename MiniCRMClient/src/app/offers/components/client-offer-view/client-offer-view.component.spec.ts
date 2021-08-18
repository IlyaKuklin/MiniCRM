import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ClientOfferViewComponent } from './client-offer-view.component';

describe('ClientOfferViewComponent', () => {
  let component: ClientOfferViewComponent;
  let fixture: ComponentFixture<ClientOfferViewComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ ClientOfferViewComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(ClientOfferViewComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
