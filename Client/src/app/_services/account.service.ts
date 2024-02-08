import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { environment } from 'src/environments/environment';
import { ReplaySubject, map } from 'rxjs';
import { LoginUserData } from '../_models/Data/loginUserData';
import { LoginUserResponse } from '../_models/Response/loginUserResponse';
import { UserInfoResponse } from '../_models/Response/userInfoResponse';
import { SearchUsersResponse } from '../_models/Response/seachUsersResponse';

@Injectable({
  providedIn: 'root'
})
export class AccountService {
  apiURL = environment.apiURL;
  private currentUserSource = new ReplaySubject<LoginUserData>(1);
  currentUser$ = this.currentUserSource.asObservable();

  constructor(private http: HttpClient) { }

  login(model: any){
    return this.http.post(this.apiURL + 'account/login', model).pipe(
      map((response: LoginUserResponse) => {
        const user = response.data;
        if(user){
          this.setCurrentUser(user);
        }
      })
    );
  }

  register(model: any){
    return this.http.post(this.apiURL + 'account/register', model);
  }

  getMyInfo(){
    return this.http.get<UserInfoResponse>(this.apiURL + 'account/MyInfo');
  }

  setCurrentUser(user: LoginUserData){
    localStorage.setItem('user', JSON.stringify(user));
    this.currentUserSource.next(user);
  }

  logout(){
    localStorage.removeItem('user');
    this.currentUserSource.next(null);
  }

  currentUser(){
    let currentUser: LoginUserData;

    if(localStorage.getItem('user') == null){
      currentUser = {
        id: null,
        userName: null,
        email: null,
        photoUrl: null,
        token: null 
      }
    }
    else{
      currentUser = JSON.parse(localStorage.getItem('user'));
    }

    return currentUser;
  }

  editUser(model: any){
    return this.http.put(this.apiURL + "account/edit", model);
  }

  changePassword(model: any){
    return this.http.put(this.apiURL + "account/changePassword", model);
  }

  changePhoto(file: any){
    return this.http.put(this.apiURL + "account/changePhoto", file);
  }
}
