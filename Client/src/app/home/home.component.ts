import { Component, OnInit } from '@angular/core';
import { AccountService } from '../_services/account.service';
import { Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.css']
})
export class HomeComponent implements OnInit {
  model: any = {};
  validationErrors: string[] = [];
  registerMode: boolean = false;

  constructor(private accountService: AccountService, private router: Router, private toastr: ToastrService) { }

  ngOnInit(): void {
  }

  login(){
    this.accountService.login(this.model).subscribe(response => {
      this.toastr.info("Logged in")
      //this.router.navigateByUrl('/boards');
    }, error => {
      this.validationErrors = error;
    });
  }

  register(){
    this.accountService.register(this.model).subscribe(response => {
      this.cancelRegister();
      // this.router.navigateByUrl("/boards");
    }, error => {
      this.validationErrors = error;
    })
  }

  startRegister(){
    this.registerMode = true;
  }

  cancelRegister(){
    this.registerMode = false;
  }
}
