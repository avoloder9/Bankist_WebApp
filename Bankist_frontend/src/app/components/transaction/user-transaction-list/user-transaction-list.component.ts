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

  constructor(private httpClient: HttpClient, private router: Router, private route: ActivatedRoute) { }
  ngOnInit(): void {
    const headers = new HttpHeaders({
      Token: localStorage.getItem('token') ?? '',
    });
    this.route.params.subscribe((params) => {
      this.bankName = params['bankName'];
      if (this.bankName) {

        this.httpClient.get<Transaction[]>(`${MyConfig.serverAddress}/GetUserTransactions?bankName=${this.bankName}`, { headers: headers })
          .subscribe(
            (data) => {

              /* this.transactions = data;
               console.log(data);
 */
              this.transactions = data.map(transaction => {
                if (transaction.senderCardId) {
                  //treba dodati -
                  transaction.amount = Math.abs(transaction.amount);
                } else if (transaction.recieverCardId) {
                  transaction.amount = Math.abs(transaction.amount);
                }
                return transaction;
              });
              console.log(data);
            },
            (error) => {
              console.error('Error fetching data:', error);
            }
          );

      }
    });
  }
}






