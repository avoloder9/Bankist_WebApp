import { ComponentFixture, TestBed } from '@angular/core/testing';

import { BankViewComponent } from './bank-view.component';

describe('BankViewComponent', () => {
  let component: BankViewComponent;
  let fixture: ComponentFixture<BankViewComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [BankViewComponent]
    });
    fixture = TestBed.createComponent(BankViewComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
