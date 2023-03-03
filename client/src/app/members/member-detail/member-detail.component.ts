import { Component, Directive, OnDestroy, OnInit, ViewChild } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { NgxGalleryAnimation, NgxGalleryImage, NgxGalleryOptions } from '@rybos/ngx-gallery';
import { TabDirective, TabsetComponent } from 'ngx-bootstrap/tabs';
import { take } from 'rxjs';
import { Memeber } from 'src/app/_models/Member';
import { Message } from 'src/app/_models/Message';
import { User } from 'src/app/_models/user';
import { AccountService } from 'src/app/_services/account.service';
import { MembersService } from 'src/app/_services/members.service';
import { MessageService } from 'src/app/_services/message.service';
import { PresenceService } from 'src/app/_services/presence.service';

@Component({
  selector: 'app-member-detail',
  templateUrl: './member-detail.component.html',
  styleUrls: ['./member-detail.component.css']
})
export class MemberDetailComponent implements OnInit,OnDestroy {
  @ViewChild('memberTabs', { static: true }) memberTabs: TabsetComponent;
  activeTab: TabDirective;
  galleryOptions: NgxGalleryOptions[];
  galleryImages: NgxGalleryImage[];
  memberId: Memeber;
  messages: Message[] = [];
  user:User;

  constructor( private messageServer: MessageService,
    public presenceService: PresenceService,
    private rout: ActivatedRoute,private router:Router,private accountService:AccountService) {
      this.accountService.currentUser$.pipe(take(1)).subscribe(res=>this.user=res);
      this.router.routeReuseStrategy.shouldReuseRoute=()=>false;
    }
 
   ngOnInit(): void {
    this.rout.data.subscribe( data => {
      this.memberId =  data.member;
     
    })
    
    this.rout.queryParams.subscribe(params => {
      params.tab? this.selectTab(params.tab) : this.selectTab(0)
    });
   
    // this.loadMember();

    this.galleryOptions = [
      {
        width: '500px',
        height: '500px',
        imagePercent: 100,
        thumbnailsColumns: 4,
        imageAnimation: NgxGalleryAnimation.Slide,
        preview: false
      }
    ];

    this.galleryImages = this.getImages();
    
  }


  getImages(): NgxGalleryImage[] {
    const imageUrl = [];
    for (const photo of this.memberId.photos)
      imageUrl.push({
        small: photo?.url,
        medium: photo?.url,
        big: photo?.url
      });
    return imageUrl;
  }

  // loadMember() {
  //   this.memberService.getMember(this.rout.snapshot.paramMap.get('userName')).subscribe(
  //     data => {
  //       this.memberId = data; console.log(data);
  //       // this.galleryImages = this.getImages();
  //     }
  //   )

  // }

  loadMessages() {
    this.messageServer.getMessagesThread(this.memberId.userName).subscribe(res => {
      this.messages = res;
    });
  }
  selectTab(tabId: number) {
    console.log("Tab is : "+tabId)
      this.memberTabs.tabs[tabId].active=true;
  }

  onTabActivated(data: TabDirective) {
    this.activeTab = data
    if (this.activeTab.heading == "Mesages" && this.messages.length === 0)
      //this.loadMessages();
      this.messageServer.createHubConnection(this.user,this.memberId.userName);
      else
      this.messageServer.stopHubConnection();
    }
    ngOnDestroy(): void {
      this.messageServer.stopHubConnection();
    }

}
