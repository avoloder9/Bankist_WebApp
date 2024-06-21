import { Component } from '@angular/core';
import { Store } from '@ngrx/store';
import { logout } from '../../shared/store/login.actions';
import { Router } from '@angular/router';
import { HttpClient } from '@angular/common/http';
import { MyConfig } from 'src/app/myConfig';
import { SignalRService } from 'src/app/services/signalR.service';
import { UserService } from 'src/app/services/UserService';
import { TranslationService } from 'src/app/services/TranslationService';
import { Location } from '@angular/common';
@Component({
  selector: 'app-navbar',
  templateUrl: './navbar.component.html',
  styleUrls: ['./navbar.component.scss'],
})
export class NavbarComponent {
  isLoggedIn: boolean;
  userId: any;
  translations: any;
  selectedLang: string = '';
  constructor(
    private store: Store<{ login: { loggedIn: boolean } }>,
    private router: Router,
    private httpClient: HttpClient,
    private userService: UserService,
    private translationService: TranslationService,
    private location: Location,
    private signalRService: SignalRService
  ) {}

  ngOnInit(): void {
    this.loadLang();
    this.translations = this.translationService.getTranslations();
    this.store.select('login').subscribe((data) => {
      this.isLoggedIn = data.loggedIn;
    });
    this.userId = this.userService.getUserId();
  }

  saveLang(): void {
    localStorage.setItem('lang', this.selectedLang);
    this.router.navigate(['/']);
    setTimeout(() => {
      window.location.reload();
    }, 200);
  }

  loadLang(): void {
    const storedLang = localStorage.getItem('lang');
    if (storedLang) {
      this.selectedLang = storedLang;
    }
  }

  logout() {
    this.httpClient
      .post(`${MyConfig.serverAddress}/Auth/logout`, {
        signalRConnection: SignalRService.ConnectionId,
      })
      .subscribe(() => {
        console.log('logout');
      });
    console.log('logout');
    this.signalRService.close_ws_connection();
    localStorage.removeItem('token');
    localStorage.removeItem('User');
    this.router.navigate(['/']);
    this.store.dispatch(logout());
  }
}
