import { Component, OnInit } from '@angular/core';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { User } from './login.interface';
import { LoginService } from './loginservice';
import { SweetAlertService } from '../shared/alert/sweetalert.service';
import { SharedService } from '../shared/shared.service';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.scss']
})
export class LoginComponent implements OnInit {
  login: FormGroup;
  emailPattern = /\S+@\S+\.\S+/;
  passwordPattern = /^(?=.*\d)(?=.*[a-z])(?=.*[A-Z])(?=.*[!@#\$%\^&\*])(?=.{9,})/;
  user: User;
  invalidUser: boolean = false;

  constructor(private router: Router,
    private loginService: LoginService,
    private sweetAlertService: SweetAlertService,
    private sharedService: SharedService
    ) {
    this.user = new User();
  }

  ngOnInit(): void { }

  signIn(): void {
    debugger;
    if (this.user.email != "" && this.user.password != "") {
      this.loginService.login(this.user).subscribe(
        userResponse => {
          debugger
          if (
            userResponse.body != null &&
            userResponse.body.token != null &&
            userResponse.body.token != ""
          ) {
            debugger;
            localStorage.setItem("token", userResponse.body.token);
            this.sharedService.setLocalStorage("userInfo", userResponse.body);

            this.router.navigateByUrl("/admin/dashboard");
          }
        },
        error => {
          this.sweetAlertService.sweetAlert('Error', error.statusText, 'error', false);
        }
      );
    } else {
      this.sweetAlertService.sweetAlert('Warning', "Please provide the valid credentials!", 'warn', false);
    }
  }
}
