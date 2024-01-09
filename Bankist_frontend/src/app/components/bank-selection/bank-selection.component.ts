import { Component } from '@angular/core';
import { Router } from '@angular/router';

interface Bank {
  name: string;
  image: string;
}

@Component({
  selector: 'app-bank-selection',
  templateUrl: './bank-selection.component.html',
  styleUrls: ['./bank-selection.component.scss'],
})
export class BankSelectionComponent {
  banks: Bank[] = [
    { name: 'Unicredit Bank', image: '../../assets/images/bank1.png' },
    { name: 'Reiffeisen Bank', image: '../assets/images/bank2.jpg' },
    { name: 'BBI Bank', image: '../assets/images/bank3.png' },
    { name: 'Intesa san paolo', image: '../assets/images/bank4.png' },
    { name: 'Addiko Bank', image: '../assets/images/bank5.png' },
  ];
  constructor(private router: Router) {}

  addNewBank() {
    this.router.navigate(['/new-bank']);
  }
}
