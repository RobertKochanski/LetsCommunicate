import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment';
import { GroupListResponse } from '../_models/groupListResponse';
import { Observable } from 'rxjs';
import { GroupResponse } from '../_models/groupResponse';

@Injectable({
  providedIn: 'root'
})
export class GroupService {
  apiURL = environment.apiURL;

  constructor(private http: HttpClient) { }

  getGroups(): Observable<GroupListResponse>{
    return this.http.get<GroupListResponse>(this.apiURL + 'Group');
  }

  getGroup(id: any){
    return this.http.get<GroupResponse>(this.apiURL + 'Group/' + id);
  }

  postGroup(model: any){
    return this.http.post(this.apiURL + "Group", model);
  }

  deleteGroup(id: any){
    return this.http.delete(this.apiURL + "Group/" + id);
  }

  grantPermission(groupId: any, model: any){
    return this.http.put(this.apiURL + "Group/GrantPermission/" + groupId, {userEmail: model});
  }

  revokePermission(groupId: any, model: any){
    return this.http.put(this.apiURL + "Group/RevokePermission/" + groupId, {userEmail: model});
  }
}
