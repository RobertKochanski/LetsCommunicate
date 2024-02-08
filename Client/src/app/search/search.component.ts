import { Component, OnInit } from '@angular/core';
import { AccountService } from '../_services/account.service';
import { ToastrService } from 'ngx-toastr';
import { SearchUserData } from '../_models/Data/searchUserData';
import { UserService } from '../_services/user.service';
import { NgForm } from '@angular/forms';

@Component({
  selector: 'app-search',
  templateUrl: './search.component.html',
  styleUrls: ['./search.component.css']
})
export class SearchComponent implements OnInit {
  user = this.accountService.currentUser();
  users: SearchUserData[];
  
  searchPhase: string = "";

  constructor(public accountService: AccountService, private userService: UserService, private toastr: ToastrService) { }

  ngOnInit(): void {
    this.loadUsers();
  }

  loadUsers(){
    this.userService.searchUsers(this.searchPhase).subscribe(users => {
      this.users = users.data.filter(x => x.id !== this.user.id)
    }, error => {
      this.toastr.error(error);
    })
  }

  resetForm(form: NgForm){
    form.reset();
    this.searchPhase = "";
    
    this.loadUsers();
  }
}
