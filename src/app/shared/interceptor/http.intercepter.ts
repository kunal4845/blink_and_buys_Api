import {
    HttpEvent,
    HttpHandler,
    HttpInterceptor,
    HttpRequest
} from "@angular/common/http";
import { Injectable } from "@angular/core";
import { Observable } from "rxjs";

@Injectable()
export class Interceptor implements HttpInterceptor {
    
    intercept(request: HttpRequest<any>, newRequest: HttpHandler): Observable<HttpEvent<any>> {
        // add authorization header to request
        debugger;
        //Get Token data from local storage
        let tokenInfo = localStorage.getItem('userInfo');

        if (tokenInfo) {
            request = request.clone({
                setHeaders: {
                    Authorization: `Bearer ${tokenInfo}`,
                    'Content-Type': 'application/x-www-form-urlencoded;charset=utf-8'
                }
            });
        }

        return newRequest.handle(request);
    }
}