import { Injectable } from "@angular/core";
import { ActivatedRouteSnapshot, CanActivate, Router, RouterStateSnapshot } from "@angular/router";
import { MyAuthService } from "src/app/services/MyAuthService";


@Injectable()
export class AuthorizationGuard implements CanActivate {

    constructor(private router: Router, private myAuthService: MyAuthService) { }

    canActivate(route: ActivatedRouteSnapshot, state: RouterStateSnapshot) {

        if (this.myAuthService.isLogiran()) {

            let isBank = this.myAuthService.isBank();
            if (isBank) {
                this.router.navigate(['/bank-view']);
                return false;
            }

            return true;
        }


        // not logged in so redirect to login page with the return url
        this.router.navigate(['/login'], { queryParams: { returnUrl: state.url } });
        return false;
    }
}