import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { RegistrationComponent } from './components/registration/registration.component';
import { LoginComponent } from './components/login/login.component';
import { AppComponent } from './app.component';
import { HttpClientModule } from '@angular/common/http';
import { BankSelectionComponent } from './components/bank-selection/bank-selection.component';
import { NewBankComponent } from './components/new-bank/new-bank.component';
import { TransactionComponent } from './components/transaction/transaction.component';
import { UserTransactionListComponent } from './components/transaction/user-transaction-list/user-transaction-list.component';
import { AtmComponent } from './components/atm/atm.component';
import { AuthorizationGuard } from './helpers/auth/authorizationGuardService';
import { BankViewComponent } from './components/bank-view/bank-view.component';
import { UserListComponent } from './components/user-list/user-list.component';
import { SettingsComponent } from './components/settings/settings.component';
import { LoanComponent } from './components/loan/loan.component';
import { ActiveLoansComponent } from './components/active-loans/active-loans.component';
import { LoanListComponent } from './components/loan-list/loan-list.component';
const routes: Routes = [
  { path: '', component: AtmComponent },
  { path: 'register', component: RegistrationComponent },
  { path: 'login', component: LoginComponent },
  {
    path: 'bank-selection',
    component: BankSelectionComponent,
    canActivate: [AuthorizationGuard],
  },
  {
    path: 'new-bank',
    component: NewBankComponent,
    canActivate: [AuthorizationGuard],
  },
  {
    path: 'transaction/:cardNumber',
    component: TransactionComponent,
    canActivate: [AuthorizationGuard],
  },
  {
    path: 'userTransactionList/:bankName',
    component: UserTransactionListComponent,
    canActivate: [AuthorizationGuard],
  },
  { path: 'bank-view', component: BankViewComponent },
  { path: 'user-list', component: UserListComponent },
  { path: 'loan-list', component: LoanListComponent },
  { path: 'settings', component: SettingsComponent },
  {
    path: 'loan/:cardNumber',
    component: LoanComponent,
    canActivate: [AuthorizationGuard],
  },
  {
    path: 'active-loans/:cardNumber',
    component: ActiveLoansComponent,
    canActivate: [AuthorizationGuard],
  },
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule],
})
export class AppRoutingModule {}
