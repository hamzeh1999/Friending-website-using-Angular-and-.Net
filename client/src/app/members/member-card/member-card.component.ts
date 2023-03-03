import { Component, Input, OnInit, ViewEncapsulation } from '@angular/core';
import { ToastrService } from 'ngx-toastr';
import { Memeber } from 'src/app/_models/Member';
import { MembersService } from 'src/app/_services/members.service';
import { PresenceService } from 'src/app/_services/presence.service';

@Component({
  selector: 'app-member-card',
  templateUrl: './member-card.component.html',
  styleUrls: ['./member-card.component.css'],
  encapsulation:ViewEncapsulation.None
})
export class MemberCardComponent implements OnInit {
  @Input() memberInput:Memeber;

  ngOnInit()
  {

  }
constructor(private memberService:MembersService,
  public presence:PresenceService,
  private toastr:ToastrService){}
addLike(member:Memeber){
this.memberService.addLike(member.userName).subscribe(()=>{
this.toastr.success('You have liked '+member.knownAs);
});
}
}
