import { Component } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { HttpClient } from '@angular/common/http';
import { passwordValidator } from '../registration/passwordValidator';
import { MyConfig } from '../myConfig';
import { LoaderComponent } from '../loader/loader.component';

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
  loginFailed: boolean = false;
  unexpectedError: boolean = false;

  constructor(private fb: FormBuilder, private httpClient: HttpClient) {
    this.loginForm = this.fb.group({
      username: ['', Validators.required],
      password: ['', [Validators.required]],
    });
  }

  isFieldInvalid(field: string) {
    return (
      this.loginForm.get(field)!.invalid && this.loginForm.get(field)!.touched
    );
  }

  onSubmit() {
    this.reset();
    if (this.loginForm.valid) {
      this.isSending = true;
      this.httpClient
        .post<any>(
          `${MyConfig.serverAddress}/UserLoginEndpoint`,
          this.loginForm.value
        )
        .subscribe({
          next: (response: any) => {
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
    this.loginFailed = false;
    this.unexpectedError = false;
  }
}
