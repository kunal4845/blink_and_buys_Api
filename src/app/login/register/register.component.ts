import { Component, OnInit } from "@angular/core";
import { NgForm } from "@angular/forms";
import { Router } from "@angular/router";
import { LoginService } from "../loginservice";
import { SweetAlertService } from "../../shared/alert/sweetalert.service";
import { User } from '../login.interface';
import { SharedService } from 'src/app/shared/shared.service';

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.scss']
})
export class RegisterComponent implements OnInit {
  spinner: boolean = false;
  isEmailExists: boolean = false;
  user: User;
  passwordPattern = /^(?=.*\d)(?=.*[a-z])(?=.*[A-Z])(?=.*[!@#\$%\^&\*])(?=.{9,})/;
  emailPattern = /\S+@\S+\.\S+/;

  constructor(
    private router: Router,
    private loginService: LoginService,
    private sweetAlertService: SweetAlertService,
    private sharedService: SharedService
  ) { }

  ngOnInit() {
    this.user = new User();
  }

  checkEmailExists(email: string): void {
    debugger
    if (email != "") {
      this.loginService.checkEmailExists(email).subscribe(
        (response: any) => {
          if (response.status === 200) {
            this.isEmailExists = response.body;
          }
        },
        (error) => {
          this.sweetAlertService.sweetAlert('Error', error, 'error', false);
        }
      );
    }
  }

  register(registerForm: NgForm): void {
    debugger;
    if (!this.isEmailExists) {
      if (this.user.email != "" && this.user.password != "" && this.user.confirmPassword != "" && this.user.name != "" &&
        this.user.password == this.user.confirmPassword) {
        this.spinner = true;
        this.loginService.register(this.user).subscribe(
          (userResponse: any) => {
            if (userResponse.status === 200) {
              debugger;

              localStorage.setItem("token", userResponse.body.token);
              this.sharedService.setLocalStorage("userInfo", userResponse.body);

              registerForm.reset();

              this.router.navigateByUrl("/admin/dashboard");
              this.sweetAlertService.sweetAlert('Success', "Registered successfully", 'success', false);
            }
            this.spinner = false;
          },
          error => {
            this.sweetAlertService.sweetAlert('Error', error.statusText, 'error', false);
            this.spinner = false;
          }
        );
      } else {
        this.sweetAlertService.sweetAlert('Warning', "Enter a different email!!", 'warn', false);
      }
    } else {
      this.sweetAlertService.sweetAlert('Warning', "Fill up the mandatory fields!!", 'warn', false);
    }
  }
}
