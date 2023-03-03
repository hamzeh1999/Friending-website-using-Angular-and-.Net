import { Component, OnInit } from '@angular/core';
import { Memeber } from '../_models/Member';
import { Pagination } from '../_models/Pagination';
import { MembersService } from '../_services/members.service';

@Component({
  selector: 'app-list',
  templateUrl: './list.component.html',
  styleUrls: ['./list.component.css']
})
export class ListComponent implements OnInit {
  members:Partial<Memeber[]>;
  predict:string='liked';
  pageNumber=1;
  pageSize=5;
  pagination:Pagination
  constructor(private memberService:MembersService){}
  ngOnInit(): void {
    this.loadLikes();
  }
  loadLikes(){
    this.memberService.getLikes(this.predict,this.pageNumber,this.pageSize).subscribe(res=>{
      this.members=res.result;
      this.pagination=res.pagination;
    })
  }

  pageChanged(event:any){
    this.pageNumber=event.page;
    this.loadLikes();
  }

}
