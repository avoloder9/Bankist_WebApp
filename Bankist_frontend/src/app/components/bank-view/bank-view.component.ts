import { HttpClient } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { MyConfig } from 'src/app/myConfig';
import { TranslationService } from 'src/app/services/TranslationService';
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
  translations: any;
  pageSize: number = 10;
  pageNumber: number = 1;
  totalPages: number = 0;
  pages: number[] = [];
  constructor(
    private httpClient: HttpClient,
    private route: ActivatedRoute,
    private translationService: TranslationService
  ) {}
  ngOnInit(): void {
    this.translations = this.translationService.getTranslations();
    const storedData = localStorage.getItem('User');
    if (storedData) {
      const parsedData = JSON.parse(storedData);
      this.bankId = parsedData.account.id;
    } else {
      console.error('Error: No data found in localStorage');
    }
    this.loadTransactions();
  }

  loadTransactions() {
    this.httpClient
      .get<Transaction[]>(
        `${MyConfig.serverAddress}/Transaction/bank-transaction?bankId=${this.bankId}&pageSize=${this.pageSize}&pageNumber=${this.pageNumber}`
      )
      .subscribe(
        (data: any) => {
          this.totalPages = data.totalPages;
          this.transactions = data.dataItems;
          if (this.transactions) {
            this.transactions.map((transaction: any) => {
              if (transaction.senderCard)
                transaction.currency =
                  transaction.senderCard.currency.currencyCode;
              else if (transaction.recieverCardId)
                transaction.currency =
                  transaction.recieverCard.currency.currencyCode;
            });
            this.sortTransactions();
          }
        },
        (error) => {
          console.error('Error fetching data:', error);
        }
      );
  }

  nextPage(): void {
    if (this.pageNumber < this.totalPages) {
      this.pageNumber++;
      this.loadTransactions();
    }
  }

  previousPage(): void {
    if (this.pageNumber > 1) {
      this.pageNumber--;
      this.loadTransactions();
    }
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
