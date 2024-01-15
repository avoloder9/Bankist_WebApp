import { ComponentFixture, TestBed } from '@angular/core/testing';

import { UserTransactionListComponent } from './user-transaction-list.component';

describe('UserTransactionListComponent', () => {
  let component: UserTransactionListComponent;
  let fixture: ComponentFixture<UserTransactionListComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [UserTransactionListComponent]
    });
    fixture = TestBed.createComponent(UserTransactionListComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
