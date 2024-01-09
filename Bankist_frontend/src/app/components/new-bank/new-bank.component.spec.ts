import { ComponentFixture, TestBed } from '@angular/core/testing';

import { NewBankComponent } from './new-bank.component';

describe('NewBankComponent', () => {
  let component: NewBankComponent;
  let fixture: ComponentFixture<NewBankComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [NewBankComponent]
    });
    fixture = TestBed.createComponent(NewBankComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
