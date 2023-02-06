import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { NgxGalleryAnimation, NgxGalleryImage, NgxGalleryOptions } from '@rybos/ngx-gallery';
import { Memeber } from 'src/app/_models/Member';
import { MembersService } from 'src/app/_services/members.service';

@Component({
  selector: 'app-member-detail',
  templateUrl: './member-detail.component.html',
  styleUrls: ['./member-detail.component.css']
})
export class MemberDetailComponent implements OnInit {
  galleryOptions: NgxGalleryOptions[];
  galleryImages: NgxGalleryImage[];
  memberId: Memeber;
  constructor(private memberService: MembersService, private rout: ActivatedRoute) { }
  ngOnInit(): void {
    this.loadMember();
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

  loadMember() {
    this.memberService.getMember(this.rout.snapshot.paramMap.get('userName')).subscribe(
      data => {
        this.memberId = data; console.log(data);
        this.galleryImages = this.getImages();
      }
    )

  }

 

}
