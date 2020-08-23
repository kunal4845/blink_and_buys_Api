import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { SharedService } from '../shared/shared.service';

@Component({
  selector: 'app-admin',
  templateUrl: './admin.component.html'
})
export class AdminComponent implements OnInit {

  constructor(private router: Router,private sharedService: SharedService) { }

  ngOnInit() {
  }

  logoutUser() {
    debugger


    localStorage.removeItem("userInfo");
    localStorage.removeItem("token");

    this.router.navigateByUrl("/login");
  }

}
