import { HttpClient } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { MyConfig } from 'src/app/myConfig';
import { TranslationService } from 'src/app/services/TranslationService';
interface BankGetUsersVM {
  banks: BankGetUsersVMDetails[];
}

interface BankGetUsersVMDetails {
  id: number;
  firstName: string;
  lastName: string;
  email: string;
  phone: string;
  birthDate: Date;
  registrationDate: Date;
  cardNumber: number;
  amount: number;
  expirationDate: Date;
}

@Component({
  selector: 'app-user-list',
  templateUrl: './user-list.component.html',
  styleUrls: ['./user-list.component.scss'],
})
export class UserListComponent implements OnInit {
  isDeleteDiv: boolean = false;
  selectedUserId: any;
  deleteReason: string = '';
  isAccountDeleted: boolean = false;
  isBlockDiv: boolean = false;
  isBlock: boolean = false;
  userList: BankGetUsersVMDetails[] | null = null;
  filterTable: string = '';
  bankId: any;
  hoveredValue: string | null = null;
  blockedCardNumbers: any = [];
  cardBlocked: boolean = false;
  showMessage: any;
  translations: any;
  constructor(
    private httpClient: HttpClient,
    private route: ActivatedRoute,
    private translationService: TranslationService
  ) {}
  ngOnInit(): void {
    this.translations = this.translationService.getTranslations();
    this.getUsers();
    this.getBlockedCards();
  }
  getUsers() {
    this.route.queryParams.subscribe(
      (params) => {
        this.bankId = params['bankId'];

        this.httpClient
          .get<BankGetUsersVM>(
            `${MyConfig.serverAddress}/Bank/get-bank-users?BankId=${this.bankId}`
          )
          .subscribe((data) => {
            this.userList = data.banks;
          });
      },
      (error) => {
        console.error('Error fetching data:', error);
      }
    );
  }
  showActualValue(event: MouseEvent, value: string): void {
    this.hoveredValue = value;
  }

  hideActualValue(): void {
    this.hoveredValue = null;
  }

  get filteredUsers(): any[] {
    if (!this.userList) {
      return [];
    }
    if (!this.filterTable) {
      return this.userList;
    }
    const searchText = this.filterTable.toLowerCase();

    return this.userList.filter((user) => {
      return (
        `
      ${user.firstName.toLowerCase()} ${user.lastName.toLowerCase()}`.includes(
          searchText
        ) ||
        `${user.lastName.toLowerCase()} ${user.firstName.toLowerCase()}`.includes(
          searchText
        ) ||
        user.cardNumber.toString().includes(searchText)
      );
    });
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

  deleteUser() {
    if (this.selectedUserId) {
      this.httpClient
        .delete(
          `${MyConfig.serverAddress}/Bank/delete-account?userId=${this.selectedUserId}&bankId=${this.bankId}&reason=${this.deleteReason}`,
          { responseType: 'text' }
        )
        .subscribe(
          () => {
            this.cancelDelete();
            this.isAccountDeleted = true;
            setTimeout(() => {
              this.isAccountDeleted = false;
            }, 1000);
            this.getUsers();
          },
          (error) => {
            console.error('Error deleting user', error);
          }
        );
    } else {
      console.error('Selected userId not found');
    }
  }

  showBlockDiv(userId: number) {
    this.isBlockDiv = true;
    this.selectedUserId = userId;
  }

  cancelBlock() {
    this.isBlockDiv = false;
  }

  blockCard() {
    if (this.selectedUserId) {
      this.httpClient
        .put(
          `${MyConfig.serverAddress}/Bank/block-card?userId=${this.selectedUserId}&bankId=${this.bankId}`,
          {},
          { responseType: 'text' }
        )
        .subscribe(
          () => {
            this.cancelBlock();
            this.isBlock = true;
            setTimeout(() => {
              this.isBlock = false;
            }, 1000);
            this.getUsers();
            this.getBlockedCards();
          },
          (error) => {
            console.error('Error blocking card', error);
          }
        );
    } else {
      console.error('Selected userId not found');
    }
  }

  getBlockedCards() {
    return this.httpClient
      .get(
        `${MyConfig.serverAddress}/Bank/get-blockedCards?bankId=${this.bankId}`
      )
      .subscribe((data) => {
        this.blockedCardNumbers = data;
      });
  }

  isCardBlocked(cardNumber: number): boolean {
    return this.blockedCardNumbers.some(
      (blockedCard: { card: { cardNumber: number } }) =>
        blockedCard.card.cardNumber === cardNumber
    );
  }
}
