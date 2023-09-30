import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment';

@Injectable({
  providedIn: 'root'
})
export class MessageService {
  apiURL = environment.apiURL;

  constructor(private http: HttpClient) { }

  postMessages(groupId: any, model: any){
    return this.http.post(this.apiURL + "Message/" + groupId, model);
  }
}
