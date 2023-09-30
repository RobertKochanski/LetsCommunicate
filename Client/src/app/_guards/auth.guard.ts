import { Injectable } from '@angular/core';
import { CanActivate, Router } from '@angular/router';
import { AccountService } from '../_services/account.service';

@Injectable({
  providedIn: 'root'
})
export class AuthGuard implements CanActivate {
  constructor(private accountService: AccountService, private router: Router){}

  canActivate(): boolean {
    let user = this.accountService.currentUser();

    if(user != null){
      return true;
    }

    this.router.navigateByUrl('/not-found');
    return false;
  }
}