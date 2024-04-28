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
const routes: Routes = [
  { path: '', component: AtmComponent },
  { path: 'register', component: RegistrationComponent },
  { path: 'login', component: LoginComponent },
  { path: 'bank-selection', component: BankSelectionComponent },
  { path: 'new-bank', component: NewBankComponent },
  { path: 'transaction/:cardNumber', component: TransactionComponent },
  {
    path: 'userTransactionList/:bankName',
    component: UserTransactionListComponent,
  },
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule],
})
export class AppRoutingModule {}
