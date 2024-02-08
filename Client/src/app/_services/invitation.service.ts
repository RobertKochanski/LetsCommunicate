import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment';
import { InvitationListResponse } from '../_models/Response/invitationListResponse';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class InvitationService {
  apiURL = environment.apiURL;

  constructor(private http: HttpClient) { }

  getInvitations(): Observable<InvitationListResponse>{
    return this.http.get<InvitationListResponse>(this.apiURL + 'Invitation');
  }

  postInvitation(model: any, groupId: any){
    return this.http.post(this.apiURL + 'Invitation/' + groupId, model);
  }

  deleteInvitation(id: any){
    return this.http.delete(this.apiURL + 'Invitation/' + id);
  }

  acceptInvitation(invitationId: any){
    return this.http.put(this.apiURL + 'Invitation/' + invitationId, null);
  }
}
