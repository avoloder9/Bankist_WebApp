import { Component } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { MyConfig } from 'src/app/myConfig';
import { MyAuthService } from 'src/app/services/MyAuthService';
import { SignalRService } from 'src/app/services/signalR.service';
import { VisualizationComponent } from '../visualization/visualization.component';
import { TranslationService } from 'src/app/services/TranslationService';
interface CardPinVM {
  cardNumber: number;
  pin: number;
}

interface TransactionVM {
  transactionDate: string;
  amount: number;
  type: string;
  recieverCardId: number;
}

@Component({
  selector: 'app-atm',
  templateUrl: './atm.component.html',
  styleUrls: ['./atm.component.scss'],
})
export class AtmComponent {
  cardNumber: string = '';
  tempCardNumber: string = '';
  pin: string = '';
  inputs: any[] = [7, 8, 9, 6, 5, 4, 3, 2, 1, '-', 0, '+'];
  specials: string[] = ['cancel', 'clear', 'confirm'];
  stage1: boolean = true;
  stage2: boolean = false;
  stage3: boolean = false;
  stageBalanceCheck: boolean = false;
  stageDeposit: boolean = false;
  stageWithdrawal: boolean = false;
  stageThank: boolean = false;
  stageError: boolean = false;
  stageTransaction: boolean = false;
  errorMessage: string = '';
  cardPin: CardPinVM | null = null;
  transaction: TransactionVM | null = null;
  balance: string = '';
  depositAmount: string = '';
  withdrawalAmount: string = '';
  transactionType: string = '';
  translations: any;

  constructor(
    private httpClient: HttpClient,
    private myAuthService: MyAuthService,
    private translationService: TranslationService
  ) {}

  ngOnInit() {
    this.translations = this.translationService.getTranslations();
  }

  inputNumber(x: number) {
    if (this.stage1) {
      this.formatCardNumber(x);
    } else if (this.stage2) {
      this.formatPin(x);
    } else if (this.stageDeposit || this.stageWithdrawal) {
      this.formatAmount(x);
    }
  }

  handleSpecial(special: string) {
    switch (special) {
      case 'cancel':
        this.logout();
        break;
      case 'confirm':
        if (this.stage2) {
          this.enterPin();
        }
        if (this.stageDeposit) {
          this.deposit();
        }
        if (this.stageWithdrawal) {
          this.withdrawal();
        }
        break;
      default:
        this.pin = '';
        this.depositAmount = '';
        this.withdrawalAmount = '';
        this.cardNumber = '';
    }
  }

  withdrawal() {
    let cleanStr = this.withdrawalAmount.replace(/,/g, '');
    let num = parseFloat(cleanStr);
    const date = new Date();
    const isoString = date.toISOString();
    this.transaction = {
      transactionDate: isoString,
      amount: -num,
      type: 'Withdrawal',
      recieverCardId: Number(this.tempCardNumber),
    };
    this.withdrawalAmount = '';
    this.stageWithdrawal = false;
    this.transactionType = 'withdrawal';
    this.stageTransaction = true;
    this.httpClient
      .post(
        `${MyConfig.serverAddress}/Transaction/deposit-withdrawal`,
        this.transaction
      )
      .subscribe(
        (data: any) => {
          this.stageTransaction = false;
          this.stageWithdrawal = false;
          this.stageThank = true;
          setTimeout(() => {
            this.logout();
          }, 4000);
        },
        (error) => {
          this.stageWithdrawal = false;
          this.stageTransaction = false;
          this.stageError = true;
          this.errorMessage = error.error.message;
          setTimeout(() => {
            this.stageError = false;
            this.stage3 = true;
          }, 4000);
        }
      );
  }

  deposit() {
    let cleanStr = this.depositAmount.replace(/,/g, '');
    let num = parseFloat(cleanStr);
    const date = new Date();
    const isoString = date.toISOString();
    this.transaction = {
      transactionDate: isoString,
      amount: num,
      type: 'Deposit',
      recieverCardId: Number(this.tempCardNumber),
    };
    this.depositAmount = '';
    this.stageDeposit = false;
    this.transactionType = 'deposit';
    this.stageTransaction = true;
    this.httpClient
      .post(
        `${MyConfig.serverAddress}/Transaction/deposit-withdrawal`,
        this.transaction
      )
      .subscribe(
        (data: any) => {
          this.stageTransaction = false;
          this.stageThank = true;
          setTimeout(() => {
            this.logout();
          }, 4000);
        },
        (error) => {
          console.log(error);
          this.stageTransaction = false;
          this.stageDeposit = false;
          this.stageError = true;
          this.errorMessage = error.error.message;
          setTimeout(() => {
            this.stageError = false;
            this.stage3 = true;
          }, 4000);
        }
      );
  }

  return() {
    this.stageBalanceCheck = false;
    this.stage3 = true;
  }

  showDeposit() {
    this.stage3 = false;
    this.stageDeposit = true;
  }

  showWithdrawal() {
    this.stage3 = false;
    this.stageWithdrawal = true;
  }

  logout() {
    this.resetAtm();
    this.httpClient.post(`${MyConfig.serverAddress}/Auth/logout`, {
      signalRConnection: SignalRService.ConnectionId,
    });
    localStorage.removeItem('token');
  }

  showBalance() {
    this.httpClient
      .get(
        `${MyConfig.serverAddress}/Card/card-balance?cardNumber=${this.tempCardNumber}`
      )
      .subscribe(
        (data: any) => {
          this.stage3 = false;
          this.stageBalanceCheck = true;
          this.balance = data.amount.toFixed(2);
        },
        (error) => {
          this.stageBalanceCheck = false;
          this.stageError = true;
          this.errorMessage = error.error.message;
          setTimeout(() => {
            this.stageError = false;
            this.stage3 = true;
          }, 4000);
        }
      );
  }

  checkCard() {
    const formatCardNumber = this.cardNumber.split(' ').join('');
    if (formatCardNumber && formatCardNumber.length === 6) {
      this.httpClient
        .get(
          `${MyConfig.serverAddress}/Card/check-card?cardNumber=${formatCardNumber}`
        )
        .subscribe(
          (data) => {
            this.stage1 = false;
            this.stage2 = true;
            this.tempCardNumber = formatCardNumber;
          },
          (error) => {
            this.stage1 = false;
            this.stageError = true;
            this.errorMessage = error.error.message;
            setTimeout(() => {
              this.stageError = false;
              this.stage1 = true;
            }, 4000);
          }
        );
      this.cardNumber = '';
    }
  }

  resetAtm() {
    this.cardNumber = '';
    this.tempCardNumber = '';
    this.pin = '';
    this.depositAmount = '';
    this.withdrawalAmount = '';
    this.stage1 = true;
    this.stage2 = false;
    this.stage3 = false;
    this.stageBalanceCheck = false;
    this.stageDeposit = false;
    this.stageWithdrawal = false;
    this.stageThank = false;
    this.stageError = false;
    this.errorMessage = '';
    this.cardPin = null;
    this.balance = '';
  }

  enterPin() {
    this.cardPin = {
      cardNumber: Number(this.tempCardNumber),
      pin: Number(this.pin),
    };
    this.pin = '';
    this.httpClient
      .post(
        `${MyConfig.serverAddress}/Card/authenticate-card-owner`,
        this.cardPin
      )
      .subscribe(
        (data: any) => {
          this.stage2 = false;
          this.stage3 = true;
          this.myAuthService.setLoginAccount(data.token.autentificationToken);
          localStorage.setItem('token', data.token.autentificationToken.value);
        },
        (error) => {
          this.stage2 = false;
          this.stageError = true;
          this.errorMessage = error.error.message;
          setTimeout(() => {
            this.stageError = false;
            this.stage2 = true;
          }, 4000);
        }
      );
  }

  formatCardNumber(newDigit: any): void {
    if (newDigit === '-' || newDigit === '+') this.return;
    let rawInput = this.cardNumber.replace(/\D/g, '');
    rawInput += newDigit.toString();
    if (rawInput.length > 6) {
      rawInput = rawInput.slice(0, 6);
    }
    const formatted = rawInput.match(/.{1,2}/g)?.join(' ') || '';
    this.cardNumber = formatted;
  }

  formatPin(newDigit: any): void {
    if (newDigit === '-' || newDigit === '+') this.return;
    let rawInput = this.pin.replace(/\D/g, '');
    rawInput += newDigit.toString();
    if (rawInput.length > 4) {
      rawInput = rawInput.slice(0, 4);
    }
    const formatted = rawInput.match(/.{1,4}/g)?.join(' ') || '';
    this.pin = formatted;
  }

  formatAmount(newDigit: any): void {
    let rawInput;
    console.log(this.stageDeposit);
    if (this.stageDeposit) {
      let cleanStr = this.depositAmount.replace(/,/g, '');
      let num = parseFloat(cleanStr);
      if (num >= 99999) return;
      if (
        this.depositAmount.length === 9 &&
        newDigit !== '+' &&
        newDigit !== '-'
      )
        return;
      rawInput = this.depositAmount.replace(/[^\d.]/g, '');
    } else {
      let cleanStr = this.withdrawalAmount.replace(/,/g, '');
      let num = parseFloat(cleanStr);
      if (num >= 99999) return;
      if (
        this.withdrawalAmount.length === 9 &&
        newDigit !== '+' &&
        newDigit !== '-'
      )
        return;
      rawInput = this.withdrawalAmount.replace(/[^\d.]/g, '');
    }

    let parts = rawInput.split('.');
    let wholePart = parts[0];
    let decimalPart = parts[1] || '00';
    if (decimalPart.length > 2) {
      decimalPart = decimalPart.slice(0, 2);
    } else if (decimalPart.length < 2) {
      decimalPart = decimalPart.padEnd(2, '0');
    }
    let rawCombined = `${wholePart}.${decimalPart}`;
    let numericValue = parseFloat(rawCombined);
    if (newDigit === '+') {
      numericValue += 1;
    } else if (newDigit === '-') {
      numericValue = Math.max(numericValue - 1, 0);
    } else if (/\d/.test(newDigit)) {
      if (decimalPart.length < 2) {
        decimalPart += newDigit.toString();
      } else {
        wholePart += newDigit.toString();
      }
      rawCombined = `${wholePart}.${decimalPart}`;
      numericValue = parseFloat(rawCombined);
    }
    let formatted = numericValue.toLocaleString(undefined, {
      minimumFractionDigits: 2,
      maximumFractionDigits: 2,
    });
    if (formatted.length > 9) {
      formatted = formatted.slice(0, 9);
    }
    if (this.stageDeposit) {
      this.depositAmount = formatted;
    } else {
      this.withdrawalAmount = formatted;
    }
  }
}
