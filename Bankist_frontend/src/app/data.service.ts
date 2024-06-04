import { Injectable } from '@angular/core';
import { BehaviorSubject } from 'rxjs';

@Injectable({
  providedIn: 'root',
})
export class DataService {
  private bankNameSource = new BehaviorSubject<string | null>(null);

  currentBankName = this.bankNameSource.asObservable();

  constructor() {}

  changeBankName(bankName: string | null) {
    this.bankNameSource.next(bankName);
  }
}
