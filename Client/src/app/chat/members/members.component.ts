import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { faUserMinus, faStar, faUserAltSlash } from '@fortawesome/free-solid-svg-icons';
import { ToastrService } from 'ngx-toastr';
import { GroupData } from 'src/app/_models/groupData';
import { UserData } from 'src/app/_models/userData';
import { AccountService } from 'src/app/_services/account.service';
import { GroupService } from 'src/app/_services/group.service';

@Component({
  selector: 'app-members',
  templateUrl: './members.component.html',
  styleUrls: ['./members.component.css']
})
export class MembersComponent implements OnInit {
  user = this.accountService.currentUser();

  group: GroupData;
  members: UserData[];

  faStar = faStar;
  faUserLeave = faUserMinus;
  faUserRemove = faUserAltSlash;

  constructor(public accountService: AccountService, private groupService: GroupService, private route: ActivatedRoute, private toastr: ToastrService, 
    private router: Router) { 
    this.router.routeReuseStrategy.shouldReuseRoute = () => false;
  }

  ngOnInit(): void {
    if(this.route.snapshot.paramMap.get('id') !== null){
      this.loadGroup();
    }
  }

  loadGroup(){
    this.groupService.getGroup(this.route.snapshot.paramMap.get('id')).subscribe(group => {
      this.group = group.data;
      this.members = group.data.users;
    })
  }

  permissionChange(checkboxValue: any, userEmail: any){
    console.log(checkboxValue.currentTarget.checked);
    if(checkboxValue.currentTarget.checked){
      // grant permission
      this.groupService.grantPermission(this.group.id, userEmail).subscribe(() => {
        this.toastr.success("Permission granted")
      }, error => {
        this.toastr.error(error);
      })
    } else {
      // revoke permission 
      this.groupService.revokePermission(this.group.id, userEmail).subscribe(() => {
        this.toastr.success("Permission revoked")
      }, error => {
        this.toastr.error(error);
      })
    }
  }

  leaveGroup(){
    if(confirm("You really want to leave '" + this.group.name + "' group?")){
      this.groupService.leaveGroup(this.group.id).subscribe(() => {
        this.toastr.info("You left group " + this.group.name);
        this.router.navigateByUrl("/chat");
      }, error => {
        this.toastr.error(error);
      })
    }
  }

  removeFromGroup(member: any){
    if(confirm("You are gonna remove this user '"+ member.userName +"' from group '" + this.group.name + "'")){
      debugger
      this.groupService.removeFromGroup(this.group.id, member.email).subscribe(() => {
        this.toastr.info("You removed user " + member.userName + " from group " + this.group.name);
        this.members = this.members.filter(x => x.userName !== member.userName);
      }, error => {
        this.toastr.error(error);
      })
    }
  }
}
