import { BrowserModule } from '@angular/platform-browser';
import { HttpClientModule } from '@angular/common/http';
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
  providers: [],
  bootstrap: [AppComponent],
})
export class AppModule {}
