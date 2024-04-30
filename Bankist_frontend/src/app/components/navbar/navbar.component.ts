import { Component } from '@angular/core';
import { Store } from '@ngrx/store';
import { logout } from '../../shared/store/login.actions';
import { Router } from '@angular/router';
import { HttpClient } from '@angular/common/http';
import { MyConfig } from 'src/app/myConfig';
import { SignalRService } from 'src/app/services/signalR.service';
@Component({
  selector: 'app-navbar',
  templateUrl: './navbar.component.html',
  styleUrls: ['./navbar.component.scss'],
})
export class NavbarComponent {
  isLoggedIn: boolean;

  constructor(
    private store: Store<{ login: { loggedIn: boolean } }>,
    private router: Router, private httpClient: HttpClient
  ) { }

  ngOnInit(): void {
    this.store.select('login').subscribe((data) => {
      this.isLoggedIn = data.loggedIn;
    });
  }

  logout() {


    this.httpClient.post(`${MyConfig.serverAddress}/Auth/logout`, {
      signalRConnection: SignalRService.ConnectionId

    }).subscribe(() => {
      console.log("logout");
    })
    localStorage.removeItem('token');
    this.router.navigate(['/']);
    this.store.dispatch(logout());
  }
}
