import { BrowserModule } from '@angular/platform-browser';
import { HTTP_INTERCEPTORS, HttpClientModule } from '@angular/common/http';
import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { RegistrationComponent } from './components/registration/registration.component';
import { RouterModule } from '@angular/router';
import { NgModule } from '@angular/core';
import { MatDialogModule } from '@angular/material/dialog';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { NavbarComponent } from './components/navbar/navbar.component';
import { LoginComponent } from './components/login/login.component';
import { LoaderComponent } from './components/loader/loader.component';
import { StoreModule } from '@ngrx/store';
import { loginReducer } from './shared/store/login.reducer';
import { BankSelectionComponent } from './components/bank-selection/bank-selection.component';
import { NewBankComponent } from './components/new-bank/new-bank.component';
import { BankFormComponent } from './components/bank-form/bank-form.component';
import { TransactionComponent } from './components/transaction/transaction.component';
import { UserTransactionListComponent } from './components/transaction/user-transaction-list/user-transaction-list.component';
import { VisualizationComponent } from './components/visualization/visualization.component';
import { AtmComponent } from './components/atm/atm.component';
import { MyAuthInterceptor } from './helpers/auth/myAuthInterceptor';
import { AuthorizationGuard } from './helpers/auth/authorizationGuardService';
import { BankViewComponent } from './components/bank-view/bank-view.component';
import { UserListComponent } from './components/user-list/user-list.component';
import { SettingsComponent } from './components/settings/settings.component';
import { LoanComponent } from './components/loan/loan.component';
import { ActiveLoansComponent } from './components/active-loans/active-loans.component';
import { TwoFactorAuthenticationComponent } from './components/two-factor-authentication/two-factor-authentication.component';
@NgModule({
  declarations: [
    AppComponent,
    RegistrationComponent,
    NavbarComponent,
    LoginComponent,
    LoaderComponent,
    BankSelectionComponent,
    NewBankComponent,
    BankFormComponent,
    TransactionComponent,
    UserTransactionListComponent,
    VisualizationComponent,
    AtmComponent,
    BankViewComponent,
    UserListComponent,
    SettingsComponent,
    LoanComponent,
    ActiveLoansComponent,
    TwoFactorAuthenticationComponent,
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    MatDialogModule,
    ReactiveFormsModule,
    HttpClientModule,
    FormsModule,
    StoreModule.forRoot({ login: loginReducer }),
  ],
  providers: [
    BankSelectionComponent,
    { provide: HTTP_INTERCEPTORS, useClass: MyAuthInterceptor, multi: true },
    AuthorizationGuard,
  ],
  bootstrap: [AppComponent],
})
export class AppModule {}
