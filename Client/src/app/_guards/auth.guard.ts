import { Injectable } from '@angular/core';
import { ActivatedRouteSnapshot, CanActivate, Router, RouterStateSnapshot, UrlTree } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { Observable } from 'rxjs';
import { AccountService } from '../_services/account.service';

@Injectable({
  providedIn: 'root'
})
export class AuthGuard implements CanActivate {
  constructor(private accountService: AccountService, private toastr: ToastrService, private router: Router){}

  canActivate(): boolean {
    let user = this.accountService.currentUser();

    if(user.token != null){
      return true;
    }

    this.router.navigateByUrl('/not-found');
    return false;
  }
}