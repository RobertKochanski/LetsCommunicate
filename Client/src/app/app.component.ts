import { Component, OnInit } from '@angular/core';
import { AccountService } from './_services/account.service';
import { ClickMode, HoverMode, MoveDirection, OutMode, Container, Engine } from 'tsparticles-engine';
import { loadSlim } from 'tsparticles-slim';
import { UserData } from './_models/userData';

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
    const user: UserData = JSON.parse(localStorage.getItem('user')!);
    this.accountService.setCurrentUser(user);
  }
}