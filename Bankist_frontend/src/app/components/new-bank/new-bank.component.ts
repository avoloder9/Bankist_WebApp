import { Component } from '@angular/core';
import { BankFormComponent } from '../bank-form/bank-form.component';
import { Router } from '@angular/router';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { MyConfig } from 'src/app/myConfig';
import { LoaderComponent } from '../loader/loader.component';
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
  isLoading: boolean = false;
  selectedBank: BankOption;
  banks: BankOption[] = [];

  constructor(private router: Router, private httpClient: HttpClient) { }

  ngOnInit() {
    this.getBanks();
  }

  getBanks() {
    console.log('GET BANKS');
    this.isLoading = true;
    const headers = new HttpHeaders({
      Token: localStorage.getItem('token') ?? '',
    });
    this.httpClient
      .get<any>(`${MyConfig.serverAddress}/Bank/unactive-banks`, {
        headers: headers,
      })
      .subscribe({
        next: (response: any) => {
          console.log(response);
          this.setBanks(response);
        },
        error: (error) => {
          console.log(error);
        },
      });
  }

  setBanks(data: any) {
    this.banks = [];
    data.banks.forEach((item: { name: any; numberOfUsers: any }) => {
      this.banks.push({
        name: item.name,
        image: `../assets/images/${item.name}.png`,
        numberOfUsers: item.numberOfUsers,
      });
    });
    this.isLoading = false;
  }

  selectBank(bank: BankOption) {
    this.selectedBank = bank;
    this.openModal();
  }
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
