import { Component } from '@angular/core';

@Component({
  selector: 'app-atm',
  templateUrl: './atm.component.html',
  styleUrls: ['./atm.component.scss'],
})
export class AtmComponent {
  cardNumber: string = '';
  pin: string = '';
  depositAmount: number = 0;
  withdrawalAmount: number = 0;
  inputs: any[] = [7, 8, 9, 6, 5, 4, 3, 2, 1, '-', 0, '+'];
  specials: string[] = ['cancel', 'clear', 'confirm'];
  formatCardNumber(event: Event): void {
    const target = event.target as HTMLInputElement;
    let rawInput = target.value;
    rawInput = rawInput.replace(/\D/g, '');
    if (rawInput.length > 16) {
      rawInput = rawInput.slice(0, 16);
    }
    const formatted = rawInput.match(/.{1,4}/g)?.join(' ') || '';
    this.cardNumber = formatted;
    target.value = this.cardNumber;
  }
  formatPin(event: Event): void {
    const target = event.target as HTMLInputElement;
    let rawInput = target.value;
    rawInput = rawInput.replace(/\D/g, '');
    if (rawInput.length > 16) {
      rawInput = rawInput.slice(0, 16);
    }
    const formatted = rawInput.match(/.{1,4}/g)?.join(' ') || '';
    this.pin = formatted;
    target.value = this.cardNumber;
  }
}
