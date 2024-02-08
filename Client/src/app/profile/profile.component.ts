import { Component, OnInit } from '@angular/core';
import { AccountService } from '../_services/account.service';
import { UserInfoData } from '../_models/Data/userInfoData';
import { ToastrService } from 'ngx-toastr';
import { NgForm } from '@angular/forms';
import { ActivatedRoute } from '@angular/router';
import { UserService } from '../_services/user.service';

@Component({
  selector: 'app-profile',
  templateUrl: './profile.component.html',
  styleUrls: ['./profile.component.css']
})
export class ProfileComponent implements OnInit {
  user = this.accountService.currentUser();
  userInfo: UserInfoData;
  editModel: any;

  passwordModel: any;
  validationErrors: string[] = [];

  imageSrc: any;

  constructor(public accountService: AccountService, private userService: UserService, private toastr: ToastrService, private route: ActivatedRoute) { }

  ngOnInit(): void {
    this.loadUserInfo();
  }

  loadUserInfo(){
    if(this.route.snapshot.paramMap.get('id') === null){
      this.accountService.getMyInfo().subscribe(userInfo => {
        this.userInfo = userInfo.data;
  
        this.editModel = {
          userName: userInfo.data.userName,
          description: userInfo.data.description,
          country: userInfo.data.country,
          city: userInfo.data.city 
        };
  
        this.passwordModel = {
          oldPassword: "",
          newPassword: "",
          confirmNewPassword: ""
        };
      }, error => {
        this.toastr.error(error);
      })
    }
    else 
    {
      this.userService.getUserById(this.route.snapshot.paramMap.get('id')).subscribe(userInfo => {
        this.userInfo = userInfo.data;
      }, error => {
        this.toastr.error(error);
      })
    }
  }

  updateUser(ngForm: NgForm){
    this.accountService.editUser(this.editModel).subscribe(() => {
      this.loadUserInfo();
      ngForm.resetForm();
      this.toastr.info("You have successfully updated your profile")
    }, error => {
      this.toastr.error(error);
    })
  }

  changePassword(ngForm: NgForm){
    this.accountService.changePassword(this.passwordModel).subscribe(() => {
      this.loadUserInfo();
      ngForm.resetForm();
      this.toastr.info("You have successfully changed your password")
    }, error => {
      this.validationErrors = error;
    })
  }

  uploadImage(event){
    const file: File = event.target.files[0];
    const formData = new FormData();
    formData.append('file', file, file.name);

    if(file){
      this.accountService.changePhoto(formData).subscribe((newPhotoUrl: any) => {
        this.user.photoUrl = newPhotoUrl.data;
        this.accountService.setCurrentUser(this.user)

        this.loadUserInfo();

        this.toastr.success("Photo changed successfully");
      }, error => {
        this.toastr.error(error);
      })
    }
  }
}
