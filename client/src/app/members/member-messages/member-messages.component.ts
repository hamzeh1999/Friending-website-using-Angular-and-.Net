import { ChangeDetectionStrategy } from '@angular/compiler';
import { Component, Input, OnInit, ViewChild } from '@angular/core';
import { NgForm } from '@angular/forms';
import { Toast, ToastrService } from 'ngx-toastr';
import { async } from 'rxjs';
import { Message } from 'src/app/_models/Message';
import { MessageService } from 'src/app/_services/message.service';
// import { ChangeDetectorRef } from '@angular/core';

@Component({
  selector: 'app-member-messages',
  templateUrl: './member-messages.component.html',
  styleUrls: ['./member-messages.component.css'],
  //  changeDetection:ChangeDetectorRef.
})
export class MemberMessagesComponent implements OnInit {
  onpush1=1;
  @Input() messages: Message[];
  @Input() userName: string
  messageContent: string = "";
  @ViewChild('messageForm') messageForm: NgForm;

  constructor(private toast: ToastrService, public messageServer: MessageService) { 
    // this.cdr.detectChanges();

  }
  ngOnInit(): void {
    //    throw new Error('Method not implemented.');
    // console.log(this.messages.length.toString());
    // console.log(this.userName);

    // for(let i=0;i<this.messages.length;i++)
    // console.log(this.messages[i].content);

    // console.log(this.messageServer.messageThread$.subscribe(()=>{
    //   console.log(this.messageServer.messageThread$);
    // }
    // ));

    // for(let message1 of (this.messageServer.messageThread$);message1++)
    // console.log(message1);


  }
  sendMessage() {
    if (this.messageContent.length != 0)
      this.messageServer.sendMessage(this.userName, this.messageContent).then(() => {
        this.messageForm.reset();
      });
    else
      this.toast.error("Empty messages cannot be sent");
  }

}
