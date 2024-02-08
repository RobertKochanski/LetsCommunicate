import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { SearchUsersResponse } from '../_models/Response/seachUsersResponse';
import { environment } from 'src/environments/environment';
import { UserInfoResponse } from '../_models/Response/userInfoResponse';

@Injectable({
  providedIn: 'root'
})
export class UserService {
  apiURL = environment.apiURL;

  constructor(private http: HttpClient) { }

  searchUsers(searchPhase: string){
    if(searchPhase.length === 0){
      return this.http.get<SearchUsersResponse>(this.apiURL + "user/searchUsers");
    } 
    else {
      return this.http.get<SearchUsersResponse>(this.apiURL + "user/searchUsers?searchPhase=" + searchPhase);
    }
  }

  getUserById(id: any){
    return this.http.get<UserInfoResponse>(this.apiURL + "user/" + id)
  }
}
