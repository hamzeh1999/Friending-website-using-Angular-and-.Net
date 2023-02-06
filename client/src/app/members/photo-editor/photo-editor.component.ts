import { Component, ElementRef, Input, OnInit, ViewChild } from '@angular/core';
import { NgForm } from '@angular/forms';
import { ToastrModule, ToastrService } from 'ngx-toastr';
import { async, Observable, take } from 'rxjs';
import { Memeber } from 'src/app/_models/Member';
import { Photo } from 'src/app/_models/Photo';
import { User } from 'src/app/_models/user';
import { AccountService } from 'src/app/_services/account.service';
import { MembersService } from 'src/app/_services/members.service';
import { UploadServiceService } from 'src/app/_services/upload-service.service';

@Component({
  selector: 'app-photo-editor',
  templateUrl: './photo-editor.component.html',
  styleUrls: ['./photo-editor.component.css']
})
export class PhotoEditorComponent implements OnInit {
  @ViewChild("editForm1", { static: false }) InputVar: ElementRef;
  @Input() member: Memeber;
  numberOfFiles: number = 0;
  file = File || [];
  user: User;
  photo: Photo;

  constructor(private accountServer: AccountService, private uploadServer: UploadServiceService, private memberServer: MembersService, private toast: ToastrService) {
    this.accountServer.currentUser$.pipe(take(1)).subscribe(data => this.user = data);

  }



  ngOnInit(): void {

  }


  onFilechange(event: any) {
    this.numberOfFiles = event.target.files.length;
    //console.log(event.target.files)
    //console.log("length :" + event.target.files.length);
    //console.log("numberOfFiles og length :" + this.numberOfFiles);

    for (let i = 0; i < event.target.files.length; i++) {
      this.file[i] = event.target.files[i]
    }

    if (this.user.photoUrl == null) {
      this.upload();
    }

  }
  upload() {

    for (let i = 0; i < this.numberOfFiles; i++) {
      if (this.file[i]) {
        if (this.file[i].type.toLowerCase().includes("jpg") || this.file[i].type.toLowerCase().includes("png") || this.file[i].type.toLowerCase().includes("jpeg"))
          this.uploadServer.uploadfile(this.file[i]).subscribe(resp => {
            this.photo = resp;
            this.member.photos.push(this.photo);
            this.toast.success("Profile Update Successfully");
            this.InputVar.nativeElement.value = "";
            if (this.user.photoUrl == null) {
              this.user.photoUrl = this.photo.url;
              this.member.photoUrl = this.photo.url;
              this.accountServer.setCurrentUser(this.user);
            }
          });
        else {
          this.InputVar.nativeElement.value = "";
          this.toast.error("Please select a Picture");

        }


      }
      else {
        this.toast.error("Please select a File first");

      }
    }
    if (this.numberOfFiles == 0)
      this.toast.error("Please select a File first");
  }

  setMainPhoto(photo: Photo) {
    this.memberServer.setMainPhoto(photo.id).subscribe(() => {
      this.user.photoUrl = photo.url;
      this.member.photoUrl = photo.url;
      this.member.photos.forEach(p => {
        if (p.isMan) p.isMan = false;
        if (p.id === photo.id) p.isMan = true
      });
      this.accountServer.setCurrentUser(this.user);

    });
  }

  userKeyboard:string="";
  deletePhoto(photoId: number, isMan: number) {
    if (isMan == 0) {
      this.memberServer.deletePhoto(photoId).subscribe(() => {
        this.member.photos = this.member.photos.filter(x => x.id !== photoId)
      })
    }
    else {
      this.toast.error("It is Main Picture");
    }

  }

 



}



