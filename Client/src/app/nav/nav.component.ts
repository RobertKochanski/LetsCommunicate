import { Component, OnInit } from '@angular/core';
import { AccountService } from '../_services/account.service';
import { ToastrService } from 'ngx-toastr';
import { Router } from '@angular/router';
import { NgForm } from '@angular/forms';

@Component({
  selector: 'app-nav',
  templateUrl: './nav.component.html',
  styleUrls: ['./nav.component.css']
})
export class NavComponent implements OnInit {
  user = this.accountService.currentUser();

  model: any = {}

  constructor(public accountService: AccountService, private toastr: ToastrService, private router:Router) { }

  ngOnInit(): void {
  }

  login(ngForm: NgForm){
    this.accountService.login(this.model).subscribe(response => {
      ngForm.reset();
      this.toastr.info("Logged in");
      this.router.navigateByUrl('/chat');
    }, error => {
      this.toastr.error(error);
    });
  }

  logout(){
    this.toastr.info("Logged out")
    this.accountService.logout();
    this.router.navigateByUrl('/');
  }
}
