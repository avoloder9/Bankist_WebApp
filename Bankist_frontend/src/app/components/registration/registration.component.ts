import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { HttpClient } from '@angular/common/http';
import { passwordValidator } from './passwordValidator';
import { MyConfig } from '../../myConfig';
import { HttpResponse } from '@angular/common/http';
import { HttpClientModule } from '@angular/common/http';
import { numberAsyncValidator } from './numberAsyncValidator';
import { AppComponent } from '../../app.component';
import { MatDialog } from '@angular/material/dialog';
import { startWithUppercaseValidator } from './startWithUppercase';
import { LoaderComponent } from '../loader/loader.component';

@Component({
  selector: 'app-registration',
  templateUrl: './registration.component.html',
  styleUrls: ['./registration.component.scss'],
})
export class RegistrationComponent implements OnInit {
  registrationForm: FormGroup;
  isSending: boolean = false;
  registrationSuccessful: boolean = false;
  registrationFailed: boolean = false;

  constructor(
    private fb: FormBuilder,
    private httpClient: HttpClient,
    private dialogRef: MatDialog
  ) {
    this.registrationForm = this.fb.group({
      firstName: ['', [Validators.required, startWithUppercaseValidator()]],
      lastName: ['', [Validators.required, startWithUppercaseValidator()]],
      email: ['', [Validators.required, Validators.email]],
      username: ['', Validators.required],
      password: ['', [Validators.required, passwordValidator()]],
      phone: ['', Validators.required, numberAsyncValidator()],
      birthDate: ['', Validators.required],
    });
  }
  ngOnInit(): void {}

  isFieldInvalid(field: string) {
    return (
      this.registrationForm.get(field)!.invalid &&
      this.registrationForm.get(field)!.touched
    );
  }

  onSubmit() {
    if (this.registrationForm.valid) {
      this.isSending = true;
      this.httpClient
        .post<any>(
          `${MyConfig.serverAddress}/UserCheckExists`,
          this.registrationForm.value
        )
        .subscribe({
          next: (response: any) => {
            if (response.exists) {
              this.isSending = false;
              this.registrationFailed = true;
              this.registrationSuccessful = false;
            } else {
              this.httpClient
                .post<any>(
                  `${MyConfig.serverAddress}/UserAddEndpoint`,
                  this.registrationForm.value,
                  { responseType: 'text' as 'json' }
                )
                .subscribe({
                  next: () => {
                    this.isSending = false;
                    this.registrationSuccessful = true;
                    this.registrationFailed = false;
                    this.registrationForm.reset();
                  },
                  error: () => {
                    this.isSending = false;
                    this.registrationSuccessful = false;
                    this.registrationFailed = true;
                  },
                });
            }
          },
        });
    }
  }
  closePopup() {
    this.dialogRef.closeAll();
  }
}
