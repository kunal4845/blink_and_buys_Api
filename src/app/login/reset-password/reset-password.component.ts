import { Component, OnInit } from '@angular/core';
import { User } from '../login.interface';
import { Router } from '@angular/router';
import { LoginService } from '../loginservice';
import { SweetAlertService } from 'src/app/shared/alert/sweetalert.service';
import { SharedService } from 'src/app/shared/shared.service';

@Component({
  selector: 'app-reset-password',
  templateUrl: './reset-password.component.html',
  styleUrls: ['./reset-password.component.scss']
})
export class ResetPasswordComponent implements OnInit {
  user: User;
  spinner: boolean = false;
  isPasswordReset: boolean = false;

  constructor(
    private router: Router,
    private loginService: LoginService,
    private sweetAlertService: SweetAlertService,
    private sharedService: SharedService) {
    this.user = new User();
  }

  ngOnInit() { }

  recoverPassword(): void {
    debugger;
    this.spinner = true;
    if (this.user.email != "" && this.user.email != null) {
      this.loginService.resetPassword(this.user.email).subscribe(
        response => {
          debugger;
          this.user = new User();
          this.spinner = false;
          this.isPasswordReset = true;
        },
        error => {
          this.spinner = false; this.sweetAlertService.sweetAlert('Error', error, 'error', false);
        }
      );
    } else {
      this.sweetAlertService.sweetAlert('Error', "Please check you email!", 'error', false);
    }
  }
}