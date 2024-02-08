import { Component, OnInit } from '@angular/core';
import { GroupData } from '../_models/Data/groupData';
import { AccountService } from '../_services/account.service';
import { GroupService } from '../_services/group.service';
import { ActivatedRoute, Router } from '@angular/router';
import { faPlusCircle, faTrash, faUserEdit, faUserMinus, faUserAltSlash } from '@fortawesome/free-solid-svg-icons';
import { NgForm } from '@angular/forms';
import { ToastrService } from 'ngx-toastr';
import { MessageService } from '../_services/message.service';
import * as uuid from 'uuid';

@Component({
  selector: 'app-chat',
  templateUrl: './chat.component.html',
  styleUrls: ['./chat.component.css']
})
export class ChatComponent implements OnInit {
  user = this.accountService.currentUser();
  group: GroupData;
  groups: GroupData[] = [];

  faPlus = faPlusCircle;
  faUsers = faUserEdit;
  faUserLeave = faUserMinus;
  faUserRemove = faUserAltSlash;
  faTrash = faTrash;

  createGroupMode: boolean = false;

  chatModel: any = {};

  constructor(public accountService: AccountService, private groupService: GroupService, private messageService: MessageService, 
    private route: ActivatedRoute, private router: Router, private toastr: ToastrService) 
  { 
    this.router.routeReuseStrategy.shouldReuseRoute = () => false;
  }

  ngOnInit(): void {
      this.loadGroup();
  }

  loadGroup(){
    if(this.route.snapshot.paramMap.get('id') === null){
      this.groupService.getGroup(uuid.NIL).subscribe(group => {
        this.group = group.data;
      })
    } else {
      this.groupService.getGroup(this.route.snapshot.paramMap.get('id')).subscribe(group => {
        this.group = group.data;
      })
    }
  }

  createMessage(ngForm: NgForm){
    this.messageService.postMessages(this.group.id, this.chatModel).subscribe(() => {
      ngForm.reset();
      this.loadGroup();
    }, error => {
      this.toastr.error(error);
    })
  }
}
