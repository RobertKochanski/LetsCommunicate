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
  faUser = faUserEdit;
  faUserLeave = faUserMinus;
  faUserRemove = faUserAltSlash;
  faTrash = faTrash;

  createGroupMode: boolean = false;

  invitationMode: boolean = false;
  invitationGroupId: any;

  model: any = {};

  constructor(public accountService: AccountService, private groupService: GroupService, private messageService: MessageService, private invitationService: InvitationService,
    private route: ActivatedRoute, private router: Router, private toastr: ToastrService) 
  { 
    this.router.routeReuseStrategy.shouldReuseRoute = () => false;
  }

  ngOnInit(): void {
    this.loadGroups();
    if(this.route.snapshot.paramMap.get('id') !== null){
      this.loadGroup();
    }
  }

  loadGroup(){
    this.groupService.getGroup(this.route.snapshot.paramMap.get('id')).subscribe(group => {
      this.group = group.data;
    })
  }

  loadGroups(){
    this.groupService.getGroups().subscribe(groups => {
      this.groups = groups.data;
      if(this.route.snapshot.paramMap.get('id') === null){
        this.group = this.groups.find(x => x.name === "General");
      }
    })
  }

  createGroup(ngForm: NgForm){
    this.groupService.postGroup(this.model).subscribe(() => {
      this.createGroupMode = false;
      ngForm.reset();
      this.loadGroups();
      this.toastr.success("Group Created");
    }, error => {
      this.toastr.error(error);
    })
  }

  groupToggle(){
    this.createGroupMode = !this.createGroupMode;
  }

  createMessage(groupId: any, ngForm: NgForm){
    this.messageService.postMessages(groupId, this.model).subscribe(() => {
      ngForm.reset();
      if(this.route.snapshot.paramMap.get('id') === null){
        this.loadGroups();
      }
      this.loadGroup();
    }, error => {
      this.toastr.error(error);
    })
  }

  invitationToggle(groupId: any, ngForm: NgForm){
    this.invitationGroupId = groupId;
    this.invitationMode = !this.invitationMode;

    ngForm.reset();
  }

  sendInvitation(ngForm: NgForm){
    debugger
    this.model = this.model;
    this.invitationService.postInvitation(this.model, this.invitationGroupId).subscribe(() => {
      ngForm.reset();
      this.toastr.success("Invitation sent");
    }, error => {
      this.toastr.error(error);
    })
  }

  leaveGroup(){
    
  }

  removeFromGroup(){

  }
}
