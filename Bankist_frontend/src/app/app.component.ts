import { Component, OnInit, Signal } from '@angular/core';
import { Router } from '@angular/router';
import { MatDialog } from '@angular/material/dialog';
import { RegistrationComponent } from './components/registration/registration.component';
import { SignalRService } from './services/signalR.service';
import { AuthLoginVM } from './components/login/authLoginVM';
import { HttpClient } from '@angular/common/http';
import { AuthLoginResponse } from './components/login/authLoginResponse';
import { MyConfig } from './myConfig';
import { MyAuthService } from './services/MyAuthService';
import { Store } from '@ngrx/store';
import { login } from './shared/store/login.actions';
@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss'],
})
export class AppComponent implements OnInit {
  buttonDisabled: boolean = false;
  constructor(
    private dialogRef: MatDialog,
    private router: Router,
    private signalRService: SignalRService,
    private httpClient: HttpClient,
    private myAuthService: MyAuthService,
    private store: Store<{ login: { loggedIn: boolean } }>
  ) {}
  ngOnInit(): void {
    this.login();
  }

  login() {
    let user: any = localStorage.getItem('User');
    if (!user) return;
    this.signalRService.open_ws_connection();
    user = JSON.parse(user);
    this.signalRService.onConnectionIdChange.subscribe(
      (connectionId: string) => {
        let loginrequest: AuthLoginVM = {
          username: user.account.username,
          password: '',
          signalRConnectionID: connectionId,
        };
        console.log(loginrequest);
        this.httpClient
          .post<AuthLoginResponse>(
            `${MyConfig.serverAddress}/Auth/login?token=${user.value}`,
            loginrequest
          )
          .subscribe({
            next: (response: any) => {
              this.myAuthService.setLoginAccount(response.autentificationToken);

              localStorage.setItem(
                'User',
                JSON.stringify(response.autentificationToken)
              );
              this.store.dispatch(login());
              setTimeout(() => {
                if (response.autentificationToken.account.isBank) {
                  this.router.navigate([
                    '/bank-view',
                    { username: loginrequest.username },
                  ]);
                } else {
                  this.router.navigate([
                    '/bank-selection',
                    { username: user.account.id },
                  ]);
                }
              }, 1000);
            },

            error: (error) => {
              console.log(error);
              console.error(error.message);
              this.router.navigate(['']);
            },
          });
      }
    );
  }

  openDialog() {
    if (!this.buttonDisabled) {
      this.buttonDisabled = true;
      const dialog = this.dialogRef.open(RegistrationComponent);

      dialog.afterClosed().subscribe(() => {
        this.buttonDisabled = false;
      });
    }
  }
}
