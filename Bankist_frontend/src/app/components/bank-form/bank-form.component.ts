import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { MyConfig } from 'src/app/myConfig';
import { LoaderComponent } from '../loader/loader.component';
import { ActivatedRoute, Router } from '@angular/router';
import { TranslationService } from 'src/app/services/TranslationService';

interface BankOption {
  name: string;
  image: string;
  numberOfUsers: number;
}
interface BankAccountVM {
  name: string;
  amount: number;
  type: string;
  currency: string;
}
@Component({
  selector: 'app-bank-form',
  templateUrl: './bank-form.component.html',
  styleUrls: ['./bank-form.component.scss'],
})
export class BankFormComponent implements OnInit {
  username: any;
  translations: any;
  constructor(
    private router: Router,
    private httpClient: HttpClient,
    private route: ActivatedRoute,
    private translationService: TranslationService
  ) {}
  ngOnInit(): void {
    this.route.params.subscribe((params) => {
      this.username = params['username'];
    });
    this.translations = this.translationService.getTranslations();
  }

  @Output() event = new EventEmitter<string>();
  @Output() refresh = new EventEmitter<string>();
  @Input() selectedBank: BankOption;
  cardOptions = ['DEBIT', 'CREDIT'];
  selectedCard: string = this.cardOptions[0];
  currencyOptions = ['BAM', 'EUR', 'USD'];
  selectedCurrency: string = this.currencyOptions[0];
  amount: number = 100;
  isLoading: boolean = false;
  success: boolean = false;

  closeModal() {
    this.event.emit('close');
  }
  register() {
    this.isLoading = true;
    if (this.amount < 100 || this.amount > 10000) {
      console.log('Invalid amount');
      return;
    }
    const newBankAccount = {
      name: this.selectedBank.name,
      amount: this.amount,
      type: this.selectedCard,
      currency: this.selectedCurrency,
    };
    /*const headers = new HttpHeaders({
      Token: localStorage.getItem('token') ?? '',
    });*/
    this.httpClient
      .post<any>(
        `${MyConfig.serverAddress}/Bank/new-account`,
        newBankAccount /*,
        { headers: headers }*/
      )
      .subscribe({
        next: (response: any) => {
          if (response) {
            console.log(response);
            this.isLoading = false;
            this.success = true;
            this.refresh.emit('refresh');
          }
        },
        error: (error) => {
          this.isLoading = false;
          console.log(error);
        },
      });
  }
  return() {
    this.router.navigate(['/bank-selection', { username: this.username }]);
  }
}
