import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { MyConfig } from 'src/app/myConfig';

interface Bank {
  name: string;
  image: string;
}

@Component({
  selector: 'app-bank-selection',
  templateUrl: './bank-selection.component.html',
  styleUrls: ['./bank-selection.component.scss'],
})
export class BankSelectionComponent implements OnInit {
  banks: Bank[] = [];
  constructor(private router: Router, private httpClient: HttpClient) {}

  ngOnInit() {
    const headers = new HttpHeaders({
      Token: localStorage.getItem('token') ?? '',
    });
    this.httpClient
      .get<any>(`${MyConfig.serverAddress}/GetActiveBanksEndpoint`, {
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
    data.banks.forEach((item: { name: any }) => {
      this.banks.push({
        name: item.name,
        image: `../assets/images/${item.name}.png`,
      });
    });
  }

  addNewBank() {
    this.router.navigate(['/new-bank']);
  }
}
