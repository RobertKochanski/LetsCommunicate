import { Component, EventEmitter, OnInit, Output } from '@angular/core';
import { DatePipe } from '@angular/common';
import { ToastrService } from 'ngx-toastr';
import { AccountService } from '../../_services/account.service';
import { BsDatepickerConfig } from 'ngx-bootstrap/datepicker';

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.css']
})
export class RegisterComponent implements OnInit {
  @Output() cancelRegister = new EventEmitter();
  model: any = {};
  validationErrors: string[] = [];

  bsConfig: Partial<BsDatepickerConfig>;
  maxDate: Date;

  constructor(private accountService: AccountService, private toastr: ToastrService, private datePipe: DatePipe) {
    this.bsConfig = {
      containerClass: 'theme-dark-blue',
      dateInputFormat: 'DD MMMM YYYY'
    }
  }

  ngOnInit(): void {
    this.maxDate = new Date;
    this.maxDate.setFullYear(this.maxDate.getFullYear() - 16)
  }

  register(){
    this.model.DateOfBirth = this.getDateOnly(this.model.DateOfBirth);
    this.accountService.register(this.model).subscribe(response => {
      this.toastr.success("Register success");
      this.cancel();
    }, error => {
      this.validationErrors = error;
    })
  }

  cancel(){
    this.cancelRegister.emit(false);
  }

  private getDateOnly(dob: string | undefined){
    if(!dob) return;
    let theDob = new Date(dob);
    return new Date(theDob.setMinutes(theDob.getMinutes()-theDob.getTimezoneOffset())).toISOString().slice(0, 10);
  }
}
