import { HttpClient } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { MyConfig } from 'src/app/myConfig';

interface Transaction {
  transactionId: number;
  transactionDate: string;
  amount: number;
  type: string;
  status: string;
  senderCardId: number;
  senderCard: any;
  recieverCardId: number;
  recieverCard: any;
  currency: any;
}

@Component({
  selector: 'app-bank-view',
  templateUrl: './bank-view.component.html',
  styleUrls: ['./bank-view.component.scss'],
})
export class BankViewComponent implements OnInit {
  transactions: Transaction[] | null = null;
  sortedTransactions: Transaction[] | null = null;
  bankId: any;
  filterTable: string = '';

  constructor(private httpClient: HttpClient, private route: ActivatedRoute) {}
  ngOnInit(): void {
    this.route.queryParams.subscribe(
      (params) => {
        this.bankId = params['bankId'];

        this.httpClient
          .get<Transaction[]>(
            `${MyConfig.serverAddress}/Transaction/bank-transaction?bankId=${this.bankId}`
          )
          .subscribe((data) => {
            this.transactions = data;
            this.transactions.map((transaction) => {
              if (transaction.senderCard)
                transaction.currency =
                  transaction.senderCard.currency.currencyCode;
              else if (transaction.recieverCardId)
                transaction.currency =
                  transaction.recieverCard.currency.currencyCode;
            });
            this.sortTransactions();
          });
      },
      (error) => {
        console.error('Error fetching data:', error);
      }
    );
  }

  sortTransactions() {
    if (this.transactions) {
      this.sortedTransactions = this.transactions.slice().sort((a, b) => {
        return (
          new Date(b.transactionDate).getTime() -
          new Date(a.transactionDate).getTime()
        );
      });
    }
  }

  get filteredTransactions(): Transaction[] {
    if (!this.sortedTransactions) {
      return [];
    }

    if (!this.filterTable) {
      return this.sortedTransactions;
    }

    const filterNumber = parseFloat(this.filterTable);

    if (isNaN(filterNumber)) {
      return [];
    }

    return this.sortedTransactions.filter(
      (transaction) =>
        transaction.senderCardId === filterNumber ||
        transaction.recieverCardId === filterNumber
    );
  }
}
