import { HttpClient } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { AutentificationToken } from "../helpers/auth/autentificationToken";

@Injectable({ providedIn: 'root' })
export class MyAuthService {
    constructor(private httpClient: HttpClient) {
    }
    isLogiran(): boolean {
        return this.getAuthorizationToken() != null;
    }

    getAuthorizationToken(): AutentificationToken | null {
        let tokenString = window.localStorage.getItem("Token") ?? "";
        try {
            return JSON.parse(tokenString);
        }
        catch (e) {
            return null;
        }
    }
    isBank(): boolean {
        return this.getAuthorizationToken()?.account.isBank ?? false
    }

    isUser(): boolean {
        return this.getAuthorizationToken()?.account.isUser ?? false
    }


    setLoginAccount(x: AutentificationToken | null) {

        if (x == null) {
            window.localStorage.setItem("Token", '');
        }
        else {
            window.localStorage.setItem("Token", JSON.stringify(x));
        }
    }


}