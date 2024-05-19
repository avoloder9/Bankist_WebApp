import { Component, OnInit } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { MyConfig } from 'src/app/myConfig';
import { UserService } from 'src/app/services/UserService';

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
  constructor(
    private httpClient: HttpClient,
    private userService: UserService
  ) {}
  ngOnInit(): void {
    this.userId = this.userService.getUserId();
    this.user = { id: this.userId };
    console.log('UserID:', this.userId);
    this.getUserId();
  }

  getUserId() {
    this.httpClient
      .get(`${MyConfig.serverAddress}/User/getById?id=${this.userId}`)
      .subscribe(
        (data: any) => {
          this.userData = data;
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
    this.getEditData();
    if (this.user && this.isSame) {
      this.httpClient
        .put(`${MyConfig.serverAddress}/User/edit`, this.user)
        .subscribe((x) => {
          this.isSuccess = true;
          setTimeout(() => {
            this.getUserId();
            this.isSuccess = false;
            this.newPassword = '';
            this.confirmPassword = '';
          }, 3000);
        });
    } else {
      this.isSame = false;
    }
  }
}
