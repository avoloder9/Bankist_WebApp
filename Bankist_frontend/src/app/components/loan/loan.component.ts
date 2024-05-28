import { Component } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { ActivatedRoute } from '@angular/router';
import { MyConfig } from 'src/app/myConfig';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { LoaderComponent } from '../loader/loader.component';
import { Location } from '@angular/common';

interface LoanRequestVM {
  amount: number;
  userCard: number;
  rates: number;
  loanTypeId: number;
}

@Component({
  selector: 'app-loan',
  templateUrl: './loan.component.html',
  styleUrls: ['./loan.component.scss'],
})
export class LoanComponent {
  options: any = null;
  activeOption: any = null;
  userCard: any = '';
  form: FormGroup;
  amountForm: FormGroup;
  loanRequest: LoanRequestVM | null = null;
  isLoading: boolean = false;
  responseMessage: string = '';
  isSuccess: boolean = false;
  isError: boolean = false;

  constructor(
    private httpClient: HttpClient,
    private location: Location,
    private route: ActivatedRoute,
    private fb: FormBuilder
  ) {
    this.form = this.fb.group({
      numberInput: [
        4,
        [Validators.required, Validators.min(1), Validators.max(30)],
      ],
    });

    this.amountForm = this.fb.group({
      loanAmountInput: [
        1000,
        [Validators.required, Validators.min(1000), Validators.max(50000)],
      ],
    });
  }

  get numberInput() {
    return this.form.get('numberInput');
  }

  get loanAmountInput() {
    return this.amountForm.get('loanAmountInput');
  }

  return() {
    this.location.back();
  }

  getOptions() {
    this.httpClient
      .get<any>(`${MyConfig.serverAddress}/Loan/get-loan-types`)
      .subscribe({
        next: (response: any) => {
          if (response) {
            this.options = response;
            this.activeOption = response[0];
          }
        },
        error: (error) => {
          console.log(error);
        },
      });
  }

  ngOnInit() {
    this.getOptions();
    this.route.params.subscribe((params) => {
      this.userCard = params['cardNumber'];
    });
  }

  submitForm() {
    if (this.form.valid && this.amountForm.valid) {
      this.loanRequest = {
        amount: this.amountForm.value.loanAmountInput,
        rates: this.form.value.numberInput,
        userCard: +this.userCard,
        loanTypeId: this.activeOption.loanTypeId,
      };
      this.isLoading = true;
      this.httpClient
        .post<any>(
          `${MyConfig.serverAddress}/Loan/request-loan`,
          this.loanRequest
        )
        .subscribe({
          next: (response: any) => {
            this.isLoading = false;
            this.responseMessage = response.message;
            this.isSuccess = true;
            this.isError = false;
            setTimeout(() => {
              this.return();
            }, 2000);
          },
          error: (error) => {
            console.log(error);
            this.isLoading = false;
            this.responseMessage = error.error.message;
            this.isError = true;
            this.isSuccess = false;
            setTimeout(() => {
              this.isError = false;
            }, 2000);
          },
        });
    }
  }
}
