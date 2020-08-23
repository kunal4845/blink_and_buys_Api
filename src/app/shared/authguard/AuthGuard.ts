import { Injectable } from '@angular/core';
import { CanActivate, ActivatedRouteSnapshot, RouterStateSnapshot, Router } from '@angular/router';

@Injectable({
    providedIn: 'root',
})
export class AuthGuard implements CanActivate {
    constructor(private router: Router) { }
   
    canActivate(route: ActivatedRouteSnapshot, state: RouterStateSnapshot) {
        debugger;
        //If token data exist, user may login to application
        if (localStorage.getItem('token')) {
            return true;
        }

        // otherwise redirect to login page with the return url
        this.router.navigate(['/login'], { queryParams: { returnUrl: state.url } });
        return false;
    }
}
