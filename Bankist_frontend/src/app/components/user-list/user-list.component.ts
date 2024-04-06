import { HttpClient } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { MyConfig } from 'src/app/myConfig';

interface BankGetUsersVM {
  banks: BankGetUsersVMDetails[];
}

interface BankGetUsersVMDetails {
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
  styleUrls: ['./user-list.component.scss']
})
export class UserListComponent implements OnInit {

  userList: BankGetUsersVMDetails[] | null = null;
  filterTable: string = '';
  bankId: any;
  hoveredValue: string | null = null;

  constructor(private httpClient: HttpClient, private route: ActivatedRoute) { }
  ngOnInit(): void {

    this.route.queryParams.subscribe((params) => {
      this.bankId = params['bankId'];

      this.httpClient.get<BankGetUsersVM>(`${MyConfig.serverAddress}/Bank/get-bank-users?BankId=${this.bankId}`)
        .subscribe(
          (data) => {
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
    return this.userList.filter(user => {
      const fullName = `${user.firstName.toLowerCase()} ${user.lastName.toLowerCase()}`;
      return user.firstName.toLowerCase().includes(searchText) ||
        user.lastName.toLowerCase().includes(searchText) ||
        fullName.includes(searchText) ||
        user.cardNumber.toString().includes(searchText)
    });
  }
}
