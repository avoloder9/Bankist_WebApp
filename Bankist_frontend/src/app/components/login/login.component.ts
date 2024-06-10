import { Component } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { HttpClient } from '@angular/common/http';
import { MyConfig } from '../../myConfig';
import { LoaderComponent } from '../loader/loader.component';
import { AuthLoginResponse } from './authLoginResponse';
import { Store } from '@ngrx/store';
import { login } from '../../shared/store/login.actions';
import { Router } from '@angular/router';
import { AuthLoginVM } from './authLoginVM';
import { MyAuthService } from 'src/app/services/MyAuthService';
import { SignalRService } from 'src/app/services/signalR.service';
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

  constructor(
    private fb: FormBuilder,
    private httpClient: HttpClient,
    private store: Store<{ login: { loggedIn: boolean } }>,
    private router: Router,
    private myAuthService: MyAuthService,
    private signalRService: SignalRService
  ) {
    this.loginForm = this.fb.group({
      username: ['', Validators.required],
      password: ['', [Validators.required]],
    });
  }

  isFieldInvalid(field: string) {
    this.unexpectedError = false;

    return (
      this.loginForm.get(field)!.invalid && this.loginForm.get(field)!.touched
    );
  }

  onSubmit() {
    console.log(this.loginForm);
    if (this.loginForm.valid) {
      this.signalRService.open_ws_connection();

      this.signalRService.onConnectionIdChange.subscribe(
        (connectionId: string) => {
          let loginrequest: AuthLoginVM = {
            username: this.loginForm.value.username,
            password: this.loginForm.value.password,
            signalRConnectionID: connectionId,
          };
          this.reset();
          this.isSending = true;

          this.httpClient
            .post<AuthLoginResponse>(
              `${MyConfig.serverAddress}/Auth/login`,
              loginrequest
            )
            .subscribe({
              next: (response: any) => {
                this.myAuthService.setLoginAccount(
                  response.autentificationToken
                );

                localStorage.setItem(
                  'token',
                  response.autentificationToken.value
                );
                this.isSending = false;
                this.store.dispatch(login());
                this.loginForm.reset();
                if (this.myAuthService.is2FActive()) {
                  this.router.navigate([
                    '/2f-authentication',
                    {
                      username: loginrequest.username,
                    },
                  ]);
                } else
                  this.router.navigate([
                    '/bank-selection',
                    { username: loginrequest.username },
                  ]);
              },

              error: (error) => {
                this.isSending = false;
                console.log(error);
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
      );
    }
  }
  reset() {
    this.isSending = false;
    this.userNotFound = false;
    this.unauthorized = false;
    this.unexpectedError = false;
  }
}
