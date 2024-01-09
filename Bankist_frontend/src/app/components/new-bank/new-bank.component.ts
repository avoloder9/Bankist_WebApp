import { Component } from '@angular/core';
import { BankFormComponent } from '../bank-form/bank-form.component';
import { Router } from '@angular/router';

interface BankOption {
  name: string;
  image: string;
  numberOfUsers: number;
}

@Component({
  selector: 'app-new-bank',
  templateUrl: './new-bank.component.html',
  styleUrls: ['./new-bank.component.scss'],
})
export class NewBankComponent {
  modalOpen: boolean = false;

  constructor(private router: Router) {}

  banks: BankOption[] = [
    {
      name: 'Unicredit Bank',
      image: '../../assets/images/bank1.png',
      numberOfUsers: 5643,
    },
    {
      name: 'Reiffeisen Bank',
      image: '../assets/images/bank2.jpg',
      numberOfUsers: 1534,
    },
    {
      name: 'BBI Bank',
      image: '../assets/images/bank3.png',
      numberOfUsers: 3131,
    },
    {
      name: 'Intesa san paolo',
      image: '../assets/images/bank4.png',
      numberOfUsers: 7641,
    },
    {
      name: 'Addiko Bank',
      image: '../assets/images/bank5.png',
      numberOfUsers: 1432,
    },
  ];

  openModal() {
    this.modalOpen = true;
  }
  closeModal() {
    this.modalOpen = false;
  }
  return() {
    this.router.navigate(['/bank-selection']);
  }
}
