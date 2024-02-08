import { Component, OnInit } from '@angular/core';
import { InvitationData } from '../_models/Data/invitationData';
import { InvitationService } from '../_services/invitation.service';
import { ToastrService } from 'ngx-toastr';
import { faFrownOpen } from '@fortawesome/free-solid-svg-icons';

@Component({
  selector: 'app-invitations',
  templateUrl: './invitations.component.html',
  styleUrls: ['./invitations.component.css']
})
export class InvitationsComponent implements OnInit {
  invitations: InvitationData[] = [];

  faSadFace = faFrownOpen;

  constructor(private invitationService: InvitationService, private toastr: ToastrService) { }

  ngOnInit(): void {
    this.loadInvitations();
  }

  loadInvitations(){
    this.invitationService.getInvitations().subscribe(invitations => {
      this.invitations = invitations.data;
    })
  }

  acceptInvitation(invitationId: any){
    this.invitationService.acceptInvitation(invitationId).subscribe(() => {
      this.invitations = this.invitations.filter(x => x.id !== invitationId);
        this.toastr.success("Invitation Accepted");
    }, error => {
      this.toastr.error(error);
    });
  }

  rejectInvitation(invitationId: any){
    if(confirm("Are you sure you want to reject the invitation?")){
      this.invitationService.deleteInvitation(invitationId).subscribe(() => {
        this.invitations = this.invitations.filter(x => x.id !== invitationId);
        this.toastr.success("Invitation Deleted");
      }, error => {
        this.toastr.error(error);
      });
    }
  }
}
