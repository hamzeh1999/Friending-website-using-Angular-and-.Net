import { Component, HostListener, OnInit, ViewChild } from '@angular/core';
import { NgForm } from '@angular/forms';
import { Toast, ToastrService } from 'ngx-toastr';
import { take } from 'rxjs';
import { Memeber } from 'src/app/_models/Member';
import { User } from 'src/app/_models/user';
import { AccountService } from 'src/app/_services/account.service';
import { MembersService } from 'src/app/_services/members.service';

@Component({
  selector: 'app-member-edit',
  templateUrl: './member-edit.component.html',
  styleUrls: ['./member-edit.component.css']
})
export class MemberEditComponent implements OnInit {
 @ViewChild('editForm') editForm:NgForm
  memberId: Memeber;
  user: User;
  
  @HostListener('window:beforeunload',['$event']) unloadNotification($event):any{
    if(this.editForm.dirty)
    $event.returnValue=true;
  }


 

  constructor(private toast: ToastrService, private memberService: MembersService, private accountService: AccountService) {
    this.accountService.currentUser$.pipe(take(1)).subscribe(data => { this.user = data; });
  }
  ngOnInit(): void {
    this.loadMember();
  }
  loadMember() {
    this.memberService.getMember(this.user.userName).subscribe(data => { this.memberId = data;
    //console.log(data);
    });
  }

  updateMember() {
    console.log(this.memberId);
    this.memberService.updateMember(this.memberId).subscribe(()=>{
      this.toast.success("Profile Update Successfully");
      this.editForm.reset(this.memberId);
    });

  }
}
