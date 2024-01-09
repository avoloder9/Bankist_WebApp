import { Component } from '@angular/core';
import { Store } from '@ngrx/store';
import { logout } from '../../shared/store/login.actions';
import { Router } from '@angular/router';
@Component({
  selector: 'app-navbar',
  templateUrl: './navbar.component.html',
  styleUrls: ['./navbar.component.scss'],
})
export class NavbarComponent {
  isLoggedIn: boolean;

  constructor(
    private store: Store<{ login: { loggedIn: boolean } }>,
    private router: Router
  ) {}

  ngOnInit(): void {
    this.store.select('login').subscribe((data) => {
      this.isLoggedIn = data.loggedIn;
    });
  }

  logout() {
    localStorage.removeItem('token');
    this.router.navigate(['/']);
    this.store.dispatch(logout());
  }
}
