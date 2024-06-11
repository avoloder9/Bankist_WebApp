import { HttpClient } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { MyConfig } from 'src/app/myConfig';
import { SignalRService } from 'src/app/services/signalR.service';
@Component({
  selector: 'app-loan-list',
  templateUrl: './loan-list.component.html',
  styleUrls: ['./loan-list.component.scss'],
})
export class LoanListComponent {
  isDeleteDiv: boolean = false;
  selectedUserId: any;
  deleteReason: string = '';
  isAccountDeleted: boolean = false;
  isBlockDiv: boolean = false;
  isBlock: boolean = false;
  loanList: any = [];
  filterTable: string = '';
  bankId: any;
  hoveredValue: string | null = null;
  blockedCardNumbers: any = [];
  cardBlocked: boolean = false;
  showMessage: any;
  constructor(
    private httpClient: HttpClient,
    private route: ActivatedRoute,
    private signalRService: SignalRService
  ) {
    this.signalRService.reloadLoans.subscribe(() => {
      this.getLoans();
    });
  }
  ngOnInit(): void {
    this.getLoans();
  }
  getLoans() {
    this.route.queryParams.subscribe(
      (params) => {
        this.bankId = params['bankId'];
        this.httpClient
          .get(`${MyConfig.serverAddress}/Loan/get-loans?BankId=${this.bankId}`)
          .subscribe((data) => {
            const loanUser: any = data;
            this.loanList = [];
            loanUser.forEach((obj: any) => {
              const { loan, user } = obj;
              loan.user = user;
              this.loanList.push(loan);
            });
          });
      },
      (error) => {
        console.error('Error fetching data:', error);
      }
    );
  }

  approveLoan(loanId: number) {
    this.httpClient
      .put<any>(
        `${MyConfig.serverAddress}/Loan/approve-loan?loanId=${loanId}`,
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

  rejectLoan(loanId: number) {
    this.httpClient
      .put<any>(
        `${MyConfig.serverAddress}/Loan/reject-loan?loanId=${loanId}`,
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

  showActualValue(event: MouseEvent, value: string): void {
    this.hoveredValue = value;
  }

  hideActualValue(): void {
    this.hoveredValue = null;
  }

  get filteredLoans(): any[] {
    if (!this.loanList) {
      return [];
    }
    if (!this.filterTable) {
      return this.loanList;
    }
    const searchText = this.filterTable.toLowerCase();

    return this.loanList.filter((loan: any) => {
      return (
        `
      ${loan.user.firstName.toLowerCase()} ${loan.user.lastName.toLowerCase()}`.includes(
          searchText
        ) ||
        `${loan.user.lastName.toLowerCase()} ${loan.user.firstName.toLowerCase()}`.includes(
          searchText
        ) ||
        loan.cardId.toString().includes(searchText)
      );
    });
  }

  loanPending(status: any) {
    return status === 'PENDING';
  }

  cancelDelete() {
    this.isDeleteDiv = false;
    this.selectedUserId = null;
    this.deleteReason = '';
  }
  showDeleteDiv(userId: number) {
    this.isDeleteDiv = true;
    this.selectedUserId = userId;
  }

  showBlockDiv(userId: number) {
    this.isBlockDiv = true;
    this.selectedUserId = userId;
  }

  cancelBlock() {
    this.isBlockDiv = false;
  }
}
