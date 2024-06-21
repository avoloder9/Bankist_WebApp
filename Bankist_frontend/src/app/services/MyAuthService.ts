import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { AutentificationToken } from '../helpers/auth/autentificationToken';

@Injectable({ providedIn: 'root' })
export class MyAuthService {
  constructor(private httpClient: HttpClient) {}
  isLogiran(): boolean {
    return this.getAuthorizationToken() != null;
  }

  getAuthorizationToken(): AutentificationToken | null {
    let tokenString = window.localStorage.getItem('User') ?? '';
    try {
      return JSON.parse(tokenString);
    } catch (e) {
      return null;
    }
  }
  isBank(): boolean {
    return this.getAuthorizationToken()?.account.isBank ?? false;
  }

  isUser(): boolean {
    return this.getAuthorizationToken()?.account.isUser ?? false;
  }
  is2FActive(): boolean {
    return this.getAuthorizationToken()?.account.is2FActive ?? false;
  }

  setLoginAccount(x: AutentificationToken | null) {
    if (x == null) {
      window.localStorage.setItem('User', '');
    } else {
      window.localStorage.setItem('User', JSON.stringify(x));
    }
  }
}
