import { Component, OnInit } from '@angular/core';
import { AccountService } from './_services/account.service';
import { LoginUserData } from './_models/Data/loginUserData';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent implements OnInit {
  title = 'Lets Communicate';

  constructor(private accountService: AccountService){}

  ngOnInit() {
    this.setCurrentUser();
  }

  setCurrentUser(){
    const user: LoginUserData = JSON.parse(localStorage.getItem('user')!);
    this.accountService.setCurrentUser(user);
  }
}