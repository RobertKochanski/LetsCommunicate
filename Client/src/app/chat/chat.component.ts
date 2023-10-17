import { Component, OnInit } from '@angular/core';
import { GroupData } from '../_models/groupData';
import { AccountService } from '../_services/account.service';
import { GroupService } from '../_services/group.service';
import { ActivatedRoute, Router } from '@angular/router';
import { faPlusCircle, faTrash, faUserEdit, faUserMinus, faUserAltSlash } from '@fortawesome/free-solid-svg-icons';
import { NgForm } from '@angular/forms';
import { ToastrService } from 'ngx-toastr';
import { MessageService } from '../_services/message.service';
import { InvitationService } from '../_services/invitation.service';

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
    this.loadGeneral();
    if(this.route.snapshot.paramMap.get('id') !== null){
      this.loadGroup();
    }
  }

  loadGeneral(){
    this.groupService.getGroups().subscribe(groups => {
      this.groups = groups.data;
      if(this.route.snapshot.paramMap.get('id') === null){
        this.group = this.groups.find(x => x.name === "General");
      }
    })
  }

  loadGroup(){
    this.groupService.getGroup(this.route.snapshot.paramMap.get('id')).subscribe(group => {
      this.group = group.data;
    })
  }

  createMessage(ngForm: NgForm){
    this.messageService.postMessages(this.group.id, this.chatModel).subscribe(() => {
      ngForm.reset();
      if(this.route.snapshot.paramMap.get('id') === null){
        this.loadGeneral();
      } else {
        this.loadGroup();
      }
    }, error => {
      this.toastr.error(error);
    })
  }
}
