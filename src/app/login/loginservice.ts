import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { HttpClient, HttpResponse } from '@angular/common/http';
import { User } from './login.interface';

@Injectable({ providedIn: 'root' })
export class LoginService {
  private APIURL: string = "https://localhost:44378/api" + "/account/";
  constructor(private http: HttpClient) { }

  checkEmailExists(email: string): Observable<HttpResponse<User>> {
    return this.http.get<User>(`${this.APIURL}/email-exists/${email}`, {
      observe: "response"
    });
  }

  register(user: User): Observable<HttpResponse<User>> {
    return this.http.post<User>(`${this.APIURL}/signUp`, user, {
      observe: "response"
    });
  }

  login(user: User): Observable<HttpResponse<User>> {
    return this.http.post<User>(`${this.APIURL}/signIn`, user, {
      observe: "response"
    });
  }

  resetPassword(email: string): Observable<HttpResponse<User>> {
    return this.http.get<User>(`${this.APIURL}/resetPassword/${email}`, {
      observe: "response"
    });
  }
}
