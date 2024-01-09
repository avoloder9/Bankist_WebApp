import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { MatDialog } from '@angular/material/dialog';
import { RegistrationComponent } from './components/registration/registration.component';
@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss'],
})
export class AppComponent {
  buttonDisabled: boolean = false;
  constructor(private dialogRef: MatDialog, private router: Router) {}
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
