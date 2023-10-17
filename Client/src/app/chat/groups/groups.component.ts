import { Component, OnInit } from '@angular/core';
import { NgForm } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { faPlusCircle, faUserEdit, faUserMinus, faUserAltSlash, faTrash } from '@fortawesome/free-solid-svg-icons';
import { ToastrService } from 'ngx-toastr';
import { GroupData } from 'src/app/_models/groupData';
import { AccountService } from 'src/app/_services/account.service';
import { GroupService } from 'src/app/_services/group.service';

@Component({
  selector: 'app-groups',
  templateUrl: './groups.component.html',
  styleUrls: ['./groups.component.css']
})
export class GroupsComponent implements OnInit {
  user = this.accountService.currentUser();

  group: GroupData;
  groups: GroupData[];

  groupModel: any = {};

  createGroupMode: boolean = false;

  faPlus = faPlusCircle;
  faUsers = faUserEdit;
  faUserLeave = faUserMinus;
  faUserRemove = faUserAltSlash;
  faTrash = faTrash;

  constructor(public accountService: AccountService, private groupService: GroupService, private route: ActivatedRoute, private toastr: ToastrService, 
    private router: Router) { }

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
    this.groupService.postGroup(this.groupModel).subscribe(() => {
      this.createGroupMode = false;
      ngForm.reset();
      this.loadGroups();
      this.toastr.success("Group Created");
    }, error => {
      this.toastr.error(error);
    })
  }

  leaveGroup(){
    
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
}
