import { Component, OnInit } from '@angular/core';
import { Message } from '../_models/Message';
import { Pagination } from '../_models/Pagination';
import { ConfirmService } from '../_services/confirm.service';
import { MessageService } from '../_services/message.service';

@Component({
  selector: 'app-messages',
  templateUrl: './messages.component.html',
  styleUrls: ['./messages.component.css']
})
export class MessagesComponent implements OnInit {
  messages: Message[];
  pageNumber = 1;
  pageSize = 5;
  container = 'unread';
  pagination: Pagination;
  loading: boolean = false;

  constructor(private messageService: MessageService, private confirmService: ConfirmService) { }
  ngOnInit(): void {
    this.loadMessage();
  }



  loadMessage() {
    this.loading = true;
    this.messageService.getMessages(this.pageNumber, this.pageSize, this.container).subscribe(res => {
      this.messages = res.result;
      this.pagination = res.pagination;
      this.loading = false;
    });

  }


  deleteMessage(id: number) {
    this.confirmService.confirm('Confirm Delete ?','Mesage will delete it ').subscribe(result => {
      if (result) {
        this.messageService.deleteMessage(id).subscribe(() => {
          this.messages.splice(this.messages.findIndex(m => m.id === id), 1);
        });

    }
  });  

  
}

pageChanged(event: any){
  if (this.pageNumber !== event.page) {
    this.pageNumber = event.page;
    this.loadMessage();
  }

}

}
