import { ComponentFixture, TestBed } from '@angular/core/testing';

import { TwoFactorAuthenticationComponent } from './two-factor-authentication.component';

describe('TwoFactorAuthenticationComponent', () => {
  let component: TwoFactorAuthenticationComponent;
  let fixture: ComponentFixture<TwoFactorAuthenticationComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [TwoFactorAuthenticationComponent]
    });
    fixture = TestBed.createComponent(TwoFactorAuthenticationComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
