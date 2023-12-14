import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { MatDialog } from '@angular/material/dialog';
import { RegistrationComponent } from './registration/registration.component';
@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss'],
})
export class AppComponent {
  buttonDisabled: boolean = false;
  constructor(private dialogRef: MatDialog) {}
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
