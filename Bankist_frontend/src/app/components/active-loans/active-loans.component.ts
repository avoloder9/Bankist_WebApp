import { Component } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { ActivatedRoute } from '@angular/router';
import { MyConfig } from 'src/app/myConfig';
import { Location } from '@angular/common';
import { TranslationService } from 'src/app/services/TranslationService';

@Component({
  selector: 'app-active-loans',
  templateUrl: './active-loans.component.html',
  styleUrls: ['./active-loans.component.scss'],
})
export class ActiveLoansComponent {
  isLoading: boolean = false;
  cardNumber: any = null;
  loans: any = [];
  translations: any;

  constructor(
    private httpClient: HttpClient,
    private location: Location,
    private route: ActivatedRoute,
    private translationService: TranslationService
  ) {}

  return() {
    this.location.back();
  }

  cancelLoan(loanId: number) {
    this.httpClient
      .put<any>(
        `${MyConfig.serverAddress}/Loan/cancel-loan?loanId=${loanId}`,
        {}
      )
      .subscribe({
        next: (response: any) => {
          if (response) {
            console.log(response);
            this.getLoans();
          }
        },
        error: (error) => {
          console.log(error);
        },
      });
  }

  getLoans() {
    this.isLoading = true;
    this.httpClient
      .get<any>(
        `${MyConfig.serverAddress}/Loan/get-user-loans?cardNumber=${this.cardNumber}`
      )
      .subscribe({
        next: (response: any) => {
          if (response) {
            console.log(response);
            this.loans = response;
            this.isLoading = false;
          }
        },
        error: (error) => {
          console.log(error);
          this.isLoading = false;
        },
      });
  }

  isPending(status: string) {
    return status === 'PENDING';
  }

  activeClass(status: string) {
    return status.toLowerCase();
  }

  ngOnInit() {
    this.route.params.subscribe((params) => {
      this.cardNumber = params['cardNumber'];
    });
    this.getLoans();
    this.translations = this.translationService.getTranslations();
  }
}
