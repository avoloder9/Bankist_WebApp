import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { HttpClient } from "@angular/common/http";
import { passwordValidator } from './passwordValidator';
import { MyConfig } from '../myConfig';
import { HttpResponse } from '@angular/common/http';
import { HttpClientModule } from '@angular/common/http';
import { numberAsyncValidator } from './numberAsyncValidator';
import { AppComponent } from '../app.component';
import { MatDialog } from '@angular/material/dialog';
import { startWithUppercaseValidator } from './startWithUppercase';

@Component({
  selector: 'app-registration',
  templateUrl: './registration.component.html',
  styleUrls: ['./registration.component.css']
})
export class RegistrationComponent implements OnInit {


  registrationForm: FormGroup;
  registrationSuccess: boolean = false;
  userExists: boolean = false;


  constructor(private fb: FormBuilder, private httpClient: HttpClient, private dialogRef: MatDialog) {

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
  ngOnInit(): void {

  }

  isFieldInvalid(field: string) {
    return (
      this.registrationForm.get(field)!.invalid &&
      this.registrationForm.get(field)!.touched
    );
  }

  onSubmit() {

    if (this.registrationForm.valid) {

      this.httpClient.post<any>(`${MyConfig.serverAddress}/UserCheckExists`, this.registrationForm.value).subscribe(
        (response: any) => {
          if (response.exists) {
            this.userExists = true;
          }
          else {

            this.httpClient.post<any>(`${MyConfig.serverAddress}/UserAddEndpoint`, this.registrationForm.value, { responseType: 'text' as 'json' }).subscribe(
              (response: HttpResponse<any>) => {
                console.log('Registration successful:', response);
                this.userExists = false;
                this.registrationSuccess = true;
                this.registrationForm.reset();
                console.log('Form reset:', this.registrationForm.value);
              },
              error => {
                console.error('Error registering user:', error);
              }

            );
            console.log('Form submitted:', this.registrationForm.value);
          }
        }
      )
    }
  }
  closePopup() {
    this.dialogRef.closeAll();
  }
}

