import { Component } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { HttpClient } from '@angular/common/http';
import { passwordValidator } from '../registration/passwordValidator';
import { MyConfig } from '../myConfig';
import { LoaderComponent } from '../loader/loader.component';
import { AuthLoginResponse } from './authLoginResponse';
import { AuthLoginRequest } from './authLoginRequest';
@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.scss'],
})
export class LoginComponent {
  loginForm: FormGroup;
  isSending: boolean = false;
  userNotFound: boolean = false;
  unauthorized: boolean = false;
  unexpectedError: boolean = false;


  /*public loginRequest: AuthLoginRequest = {
    username: "",
    password: ""
  };*/

  constructor(private fb: FormBuilder, private httpClient: HttpClient) {
    this.loginForm = this.fb.group({
      username: ['', Validators.required],
      password: ['', [Validators.required]],
    });
  }
  ngOnInit(): void {

  }
  isFieldInvalid(field: string) {
    this.unexpectedError = false;

    return (
      this.loginForm.get(field)!.invalid && this.loginForm.get(field)!.touched
    );
  }

  onSubmit() {
    console.log(this.loginForm)
    if (this.loginForm.valid) {
      console.log("nesto");
      let loginrequest: AuthLoginRequest = {
        username: this.loginForm.value.username,
        password: this.loginForm.value.password
      }

      this.reset();
      this.isSending = true;
      this.httpClient.post<AuthLoginResponse>(`${MyConfig.serverAddress}/auth`, loginrequest)
        .subscribe({
          next: (response: any) => {

            localStorage.setItem("token", response.autentificationToken.value)
            console.log(localStorage.getItem("token"))
            this.isSending = false;
            console.log(response);
          },

          error: (error) => {
            this.isSending = false;
            console.error(error.message);
            switch (error.status) {
              case 401:
                this.unauthorized = true;
                break;
              case 404:
                this.userNotFound = true;
                break;
              default:
                this.unexpectedError = true;
            }
          },
        });

    }
  }
  reset() {
    this.isSending = false;
    this.userNotFound = false;
    this.unauthorized = false;
    this.unexpectedError = false;
  }

}

