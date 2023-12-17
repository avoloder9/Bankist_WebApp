import { HttpClient } from "@angular/common/http";
import { Injectable } from "@angular/core";

@Injectable({ providedIn: 'root' })
export class MyAuthService {
    constructor(private httpClient: HttpClient) {
    }
    isLogin(): boolean {
        let token = window.localStorage.getItem("my-auth-token");

        return token != "";
    }
}