import { Component, OnInit, Signal } from '@angular/core';
import { Router } from '@angular/router';
import { MatDialog } from '@angular/material/dialog';
import { RegistrationComponent } from './components/registration/registration.component';
import { SignalRService } from './services/signalR.service';
@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss'],
})
export class AppComponent implements OnInit {
  buttonDisabled: boolean = false;
  constructor(private dialogRef: MatDialog, private router: Router) { }
  ngOnInit(): void {

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
