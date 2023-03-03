import { Component, OnInit } from '@angular/core';
import { MembersService } from 'src/app/_services/members.service';
import { Memeber } from 'src/app/_models/Member';
import { Observable, take } from 'rxjs';
import { Pagination } from 'src/app/_models/Pagination';
import { UserParms } from 'src/app/_models/UserParms';
import { User } from 'src/app/_models/user';
import { AccountService } from 'src/app/_services/account.service';

@Component({
  selector: 'app-member-list',
  templateUrl: './member-list.component.html',
  styleUrls: ['./member-list.component.css']
})
export class MemberListComponent implements OnInit {
  members: Memeber[];
  pagination: Pagination;
  userParams: UserParms;
  user: User;
  genderList = [{ value: 'male', display: 'Males' }, { value: 'female', display: 'Females' }];
  // pageSize=5;
  // pageNumber=1
  // members$: Observable<Memeber[]>;

  reastFilter() {
    this.userParams = this.memberService.resetUserParams(); 
    this.loadMembers();
  }

  ngOnInit(): void {
    // this.members$=this.memberService.getMembers();
    this.loadMembers();
  }
  constructor(private memberService: MembersService) {
    this.userParams=this.memberService.getUserParams();
  }

  loadMembers() {
    console.log("in load Members function : ");

    this.memberService.getMembers(this.userParams).subscribe(response => {
      this.members = response.result;
      this.pagination = response.pagination

    })
  }

  pageChanged(event: any) {
    this.userParams.pageNumber = event.page;
    console.log("Page Changed Here");
    this.memberService.setUserParams(this.userParams)
    console.log("Page Chaned here : "+this.userParams);
    this.loadMembers();
  }



}
