import { Component, OnInit } from '@angular/core';
import { NgForm } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { faPlusCircle, faUserEdit, faUserMinus, faUserAltSlash, faTrash, faUserPlus } from '@fortawesome/free-solid-svg-icons';
import { ToastrService } from 'ngx-toastr';
import { GroupData } from 'src/app/_models/Data/groupData';
import { AccountService } from 'src/app/_services/account.service';
import { GroupService } from 'src/app/_services/group.service';
import { InvitationService } from 'src/app/_services/invitation.service';

@Component({
  selector: 'app-groups',
  templateUrl: './groups.component.html',
  styleUrls: ['./groups.component.css']
})
export class GroupsComponent implements OnInit {
  user = this.accountService.currentUser();

  groups: GroupData[];

  groupModel: any = {};
  invitationModel: any = {};

  createGroupMode: boolean = false;
  invitationMode: boolean = false;

  groupIdToInvite: any;

  faPlus = faPlusCircle;
  faUserPlus = faUserPlus;
  faUsers = faUserEdit;
  faUserLeave = faUserMinus;
  faUserRemove = faUserAltSlash;
  faTrash = faTrash;

  constructor(public accountService: AccountService, private groupService: GroupService, private invitationService: InvitationService,
    private route: ActivatedRoute, private toastr: ToastrService, private router: Router) { }

  ngOnInit(): void {
    this.loadGroups();
  }

  loadGroups(){
    this.groupService.getGroups().subscribe(groups => {
      this.groups = groups.data;
    })
  }

  createGroup(ngForm: NgForm){
    this.groupService.postGroup(this.groupModel).subscribe(() => {
      this.createGroupMode = false;
      ngForm.reset();
      this.loadGroups();
      this.toastr.success("Group Created");
    }, error => {
      this.toastr.error(error);
    })
  }

  leaveGroup(group: any){
    if(confirm("You really want to leave '" + group.name + "' group?")){
      this.groupService.leaveGroup(group.id).subscribe(() => {
        this.toastr.info("You leave group " + group.name);
        this.groups = this.groups.filter(x => x.id !== group.id);

        if(this.route.snapshot.paramMap.get('id') === group.id){
          this.router.navigateByUrl("/chat");
        }
      }, error => {
        this.toastr.error(error);
      })
    }
  }

  deleteGroup(group: any){
    if(confirm("You really want to delete '" + group.name + "' group?")){
      this.groupService.deleteGroup(group.id).subscribe(() => {
        this.toastr.success("Group deleted successfully")
        this.groups = this.groups.filter(x => x.id !== group.id);
      }, error => {
        this.toastr.error(error);
      })
    }
  }

  groupToggle(){
    this.createGroupMode = !this.createGroupMode;
  }

  sendInvitation(ngForm: NgForm){
    this.invitationService.postInvitation(this.invitationModel, this.groupIdToInvite).subscribe(() => {
      this.toastr.info("You send invitation to " + this.invitationModel.invitedEmail + " successfully");
      ngForm.reset();
    }, error => {
      this.toastr.error(error);
    })
  }

  invitationToggle(groupId: any, ngForm: NgForm){
    this.groupIdToInvite = groupId;
    this.invitationMode = !this.invitationMode;
    if(ngForm !== null){
      ngForm.resetForm();
    }
  }
}
