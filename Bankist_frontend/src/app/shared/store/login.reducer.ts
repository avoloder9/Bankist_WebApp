import { createReducer, on } from '@ngrx/store';
import { login, logout } from './login.actions';
import { initialState } from './login.state';
import { Router } from '@angular/router';

const _loginReducer = createReducer(
  initialState,
  on(login, (state: any) => {
    return {
      ...state,
      loggedIn: true,
    };
  }),
  on(logout, (state: any) => {
    return {
      ...state,
      loggedIn: false,
    };
  })
);

export function loginReducer(state: any, action: any) {
  return _loginReducer(state, action);
}
