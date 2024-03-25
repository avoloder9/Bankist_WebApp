import { Component, Input, OnInit } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { MyConfig } from 'src/app/myConfig';
import { ActivatedRoute, Router } from '@angular/router';

interface Transaction {
  transactionId: number;
  transactionDate: string;
  amount: number;
  type: string;
  status: string;
  senderCardId: number;
  recieverCardId: number;
}
interface Bank {
  bankName: string;
  image: string;
}

@Component({
  selector: 'app-user-transaction-list',
  templateUrl: './user-transaction-list.component.html',
  styleUrls: ['./user-transaction-list.component.scss']
})

export class UserTransactionListComponent implements OnInit {
  banks: Bank[] = [];
  transactions: Transaction[] | null = null;
  bankName: string | null = null;
  cardInfo: any;
  today: Date = new Date();

  constructor(private httpClient: HttpClient, private route: ActivatedRoute) { }
  ngOnInit(): void {

    const headers = this.getHeaders();
    console.log(headers);
    this.route.params.subscribe((params) => {
      this.bankName = params['bankName'];
      if (this.bankName) {
        this.getCardInfo();

        this.httpClient.get<Transaction[]>(`${MyConfig.serverAddress}/Transaction/user-transaction?bankName=${this.bankName}`, { headers: headers })
          .subscribe(
            (data) => {

              /* this.transactions = data;
               console.log(data);
 */
              console.log(data);

              this.transactions = data.map(transaction => {
                console.log("kartica: ", this.cardInfo.cardNumber);
                if (transaction.senderCardId === this.cardInfo.cardNumber) {
                  transaction.amount = -Math.abs(transaction.amount);
                  // Promijenite predznak na suprotni
                }
                return transaction;
              });
              console.log(this.transactions);
            },
            (error) => {
              console.error('Error fetching data:', error);
            }
          );

      }
    });
  }

  getCardInfo() {
    const headers = this.getHeaders();
    this.httpClient.get<any>(`${MyConfig.serverAddress}/Card/card-info?bankName=${this.bankName}`, { headers: headers })
      .subscribe(data => {
        this.cardInfo = data;
      }, error => {
        console.log('Greška prilikom dohvatanja podataka:', error);
      });
  }
  getHeaders(): HttpHeaders {
    const token = localStorage.getItem('token') ?? '';

    const headers = new HttpHeaders({
      Token: token
    });
    return headers;
  }
}


