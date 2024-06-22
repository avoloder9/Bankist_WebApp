import { Component, OnInit } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { MyConfig } from 'src/app/myConfig';
import { UserService } from 'src/app/services/UserService';
import { DataService } from 'src/app/data.service';
import { TranslationService } from 'src/app/services/TranslationService';
import { LoaderComponent } from '../loader/loader.component';
interface Users {
  userId: number;
  userName: string;
  firstName: string;
  lastName: string;
  email: string;
  phone: string;
  birthDate: string;
  registrationDate: string;
}
interface UserEditVM {
  id: number;
  userName?: string;
  firstName?: string;
  lastName?: string;
  email?: string;
  password?: string;
  transactionLimit: number;
  atmLimit: number;
  negativeLimit: number;
}

@Component({
  selector: 'app-settings',
  templateUrl: './settings.component.html',
  styleUrls: ['./settings.component.scss'],
})
export class SettingsComponent implements OnInit {
  userId: any;
  userData: any;
  user: UserEditVM | null = null;
  newPassword: string = '';
  confirmPassword: string = '';
  isSuccess: boolean = false;
  isSame: boolean = false;
  isPasswordMatching: boolean = true;
  transactionLimit: number;
  atmLimit: number;
  negativeLimit: number;
  bankName: string | null = null;
  translations: any;
  isLoading: boolean = false;

  constructor(
    private httpClient: HttpClient,
    private userService: UserService,
    private dataService: DataService,
    private translationService: TranslationService
  ) {}
  ngOnInit(): void {
    this.translations = this.translationService.getTranslations();
    this.dataService.currentBankName.subscribe((bankName) => {
      this.bankName = bankName;
      if (bankName) {
        localStorage.setItem('bankName', bankName);
      }
    });
    this.userId = this.userService.getUserId();
    if (this.userId) {
      localStorage.setItem('userId', this.userId);
    }
    this.userId = localStorage.getItem('userId') || '';
    this.bankName = localStorage.getItem('bankName') || '';
    console.log('UserID:', this.userId);
    this.getUserId();
  }

  getUserId() {
    this.httpClient
      .get(
        `${MyConfig.serverAddress}/User/getById?id=${this.userId}&bankName=${this.bankName}`
      )
      .subscribe(
        (data: any) => {
          this.userData = data;
          if (this.userData) {
            this.transactionLimit = this.userData.users[0].transactionLimit;
            this.atmLimit = this.userData.users[0].atmLimit;
            this.negativeLimit = this.userData.users[0].negativeLimit;
          }
          console.log(this.userData);
        },
        (error: any) => {
          console.error('Error fetching user data', error);
        }
      );
    console.log('User id', this.userId);
  }
  getEditData() {
    this.user = {
      id: this.userId,
      userName: this.userData.users[0].userName,
      firstName: this.userData.users[0].firstName,
      lastName: this.userData.users[0].lastName,
      email: this.userData.users[0].email,
      password: this.userData.users[0].password,
      transactionLimit: this.transactionLimit,
      atmLimit: this.atmLimit,
      negativeLimit: this.negativeLimit,
    };

    if (this.newPassword !== '' && this.confirmPassword !== '') {
      if (this.newPassword === this.confirmPassword) {
        this.user.password = this.newPassword;
        this.isSame = true;
        this.isPasswordMatching = true;
      } else {
        this.isSame = false;
        this.isPasswordMatching = false;
      }
    }
  }

  editUser() {
    this.isLoading = true;
    this.getEditData();
    if (
      this.user &&
      (this.isSame || (this.newPassword === '' && this.confirmPassword === ''))
    ) {
      this.httpClient
        .put(
          `${MyConfig.serverAddress}/User/edit?bankName=${this.bankName}`,
          this.user
        )
        .subscribe((x) => {
          this.isSuccess = true;
          this.isLoading = false;
          setTimeout(() => {
            this.getUserId();
            this.isSuccess = false;
            this.newPassword = '';
            this.confirmPassword = '';
          }, 1000);
        });
    } else {
      this.isSame = false;
      this.isLoading = false;
    }
  }
}
