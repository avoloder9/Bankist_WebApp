import { Component, OnInit } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { MyConfig } from 'src/app/myConfig';
import { ActivatedRoute, Router } from '@angular/router';
import { SignalRService } from 'src/app/services/signalR.service';
import { UserService } from 'src/app/services/UserService';
import { DataService } from 'src/app/data.service';
import { TranslationService } from 'src/app/services/TranslationService';

interface Transaction {
  transactionId: number;
  transactionDate: string;
  amount: number;
  type: string;
  status: string;
  senderCardId: number;
  senderCard: Card;
  recieverCardId: number;
  recieverCard: Card;
  currency: Currency;
}
interface Bank {
  bankName: string;
  image: string;
}
interface Card {
  cardNumber: number;
  expirationDate: Date;
  issueDate: Date;
  amount: number;
  pin: number;
  currencyId: number;
  currency: Currency;
}
interface Currency {
  currencyId: number;
  currencyCode: string;
  currencyName: string;
  symbol: string;
  exchangeRate: number;
}

@Component({
  selector: 'app-user-transaction-list',
  templateUrl: './user-transaction-list.component.html',
  styleUrls: ['./user-transaction-list.component.scss'],
})
export class UserTransactionListComponent implements OnInit {
  isDeleteDiv: boolean = false;
  deleteReason: string = '';
  isAccountDeleted: boolean = false;
  banks: Bank[] = [];
  transactions: Transaction[] | null = null;
  bankName: string | null = null;
  cardInfo: any;
  today: Date = new Date();
  username: any;
  userId: any;
  translations: any;
  pageSize: number = 10;
  pageNumber: number = 1;
  totalPages: number = 0;
  pages: number[] = [];
  constructor(
    private httpClient: HttpClient,
    private route: ActivatedRoute,
    private signalRService: SignalRService,
    private router: Router,
    private dataService: DataService,
    private userService: UserService,
    private translationService: TranslationService
  ) {
    this.signalRService.reloadTransactions.subscribe(() => {
      this.loadTransactions();
      this.getCardInfo();
    });
  }
  ngOnInit(): void {
    this.translations = this.translationService.getTranslations();
    this.route.params.subscribe((params) => {
      if (params['bankName']) {
        this.bankName = params['bankName'];
      } else {
        const storedData = localStorage.getItem('User');
        if (storedData) {
          const parsedData = JSON.parse(storedData);
          this.bankName = parsedData.bankName;
        } else {
          console.error('Error: No data found in localStorage');
        }
      }
      if (this.bankName) {
        this.dataService.changeBankName(this.bankName);
        this.loadTransactions();
        this.getCardInfo();
      }
      this.username = params['username'];
    });
  }
  loadTransactions() {
    this.httpClient
      .get<Transaction[]>(
        `${MyConfig.serverAddress}/Transaction/user-transaction?bankName=${this.bankName}&pageSize=${this.pageSize}&pageNumber=${this.pageNumber}`
      )
      .subscribe(
        (data: any) => {
          this.totalPages = data.totalPages;
          this.transactions = data.dataItems.map((transaction: any) => {
            if (transaction.senderCardId === this.cardInfo?.cardNumber) {
              transaction.amount = -Math.abs(transaction.amount);
              transaction.currency = transaction.senderCard.currency;
            } else if (
              transaction.recieverCardId === this.cardInfo?.cardNumber
            ) {
              transaction.currency = transaction.recieverCard.currency;
            }
            return transaction;
          });
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

  getCardInfo() {
    /*const headers = this.getHeaders();*/
    this.httpClient
      .get<any>(
        `${MyConfig.serverAddress}/Card/card-info?bankName=${this.bankName}` /*{ headers: headers }*/
      )
      .subscribe(
        (data) => {
          this.cardInfo = data;
          this.userId = this.cardInfo.userId;
          this.userService.setUserId(this.userId);
        },
        (error) => {
          console.log('Error fetching data:', error);
        }
      );
  }
  /*getHeaders(): HttpHeaders {
    const token = localStorage.getItem('token') ?? '';
    
    const headers = new HttpHeaders({
      User: token
    });
    return headers;
  }*/

  closeAccount() {
    this.httpClient
      .delete(
        `${MyConfig.serverAddress}/User/close-account?username=${this.username}&bankName=${this.bankName}&reason=${this.deleteReason}`,
        { responseType: 'text' }
      )
      .subscribe(
        () => {
          this.cancelDelete();
          this.isAccountDeleted = true;
          setTimeout(() => {
            this.isAccountDeleted = false;
            this.router.navigate([
              '/bank-selection',
              { username: this.username },
            ]);
          }, 2000);
        },
        (error) => {
          console.error('Error deleting user', error);
        }
      );
  }

  cancelDelete() {
    this.isDeleteDiv = false;
    this.deleteReason = '';
  }

  showDiv() {
    this.isDeleteDiv = true;
  }

  getStatus() {
    return `../../../../assets/images/${
      this.cardInfo?.status.toLowerCase() ?? 'bronze'
    }_status.png`;
  }
}
