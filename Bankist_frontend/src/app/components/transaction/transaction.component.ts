import { HttpClient } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { MyConfig } from '../../myConfig';
import { Location } from '@angular/common';

@Component({
  selector: 'app-transaction',
  templateUrl: './transaction.component.html',
  styleUrls: ['./transaction.component.scss'],
})
export class TransactionComponent implements OnInit {
  goBack() {
    this.location.back();
  }

  transactionForm: FormGroup;
  isExecute: boolean = false;
  unexpectedError: boolean = false;
  insufficientFunds: boolean = false;
  transactionSuccessful: boolean = false;

  constructor(private fb: FormBuilder, private httpClient: HttpClient, private location: Location) { }

  ngOnInit(): void {
    this.transactionForm = this.fb.group({
      senderCardId: ['', Validators.required],
      recieverCardId: ['', Validators.required],
      amount: ['', [Validators.required, Validators.min(1)]],
      type: ['', Validators.required],
      status: ['', Validators.required],
    });
  }

  onSubmit() {
    if (this.transactionForm.valid) {
      this.reset();
      this.isExecute = true;
      this.httpClient
        .post<any>(
          `${MyConfig.serverAddress}/Transaction/execute`,
          this.transactionForm.value
        )
        .subscribe({
          next: (response: any) => {
            this.isExecute = false;
            this.transactionSuccessful = true;
            this.transactionForm.reset();
            this.location.back();
            console.log(response);
          },
          error: (error) => {
            this.isExecute = false;
            if (error.status === 500) {
              this.unexpectedError = false;
              this.insufficientFunds = true;
              this.transactionSuccessful = false;
            } else this.unexpectedError = true;
          },
        });
    }
  }

  reset() {
    this.isExecute = false;
    this.insufficientFunds = false;
    this.transactionSuccessful = false;
  }
}
