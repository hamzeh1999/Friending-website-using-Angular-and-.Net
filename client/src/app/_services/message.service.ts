import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { HubConnection, HubConnectionBuilder } from '@microsoft/signalr';
import { environment } from 'enviroments/environment';
import { BehaviorSubject, take } from 'rxjs';
import { Group } from '../_models/Group';
import { Message } from '../_models/Message';
import { User } from '../_models/user';
import { getPaginatedResult, getPaginationHeader } from './PaginationHealper';

@Injectable({
  providedIn: 'root'
})
export class MessageService {
  baseUrl = environment.apiUrl;
  hubUrl = environment.hubUrl;
  private hubConnection: HubConnection;
  private messageThreadSource = new BehaviorSubject<Message[]>([]);
  messageThread$ = this.messageThreadSource.asObservable();

  createHubConnection(user: User, otherUserName: string) {

console.log("In creat Hub Connection ");

    this.hubConnection = new HubConnectionBuilder().withUrl(this.hubUrl + 'message?user=' + otherUserName, {
      accessTokenFactory: () => user.token
    }).withAutomaticReconnect().build();

    this.hubConnection.start().catch(error => console.log(error));

    this.hubConnection.on('ReciveMessageThread', res => {
      console.log("In creat Hub Connection Recive Message Thread ");

      this.messageThreadSource.next(res);
    });

    this.hubConnection.on('NewMessage', res => {
      console.log("In creat Hub Connection New Message ");

      this.messageThread$.pipe(take(1)).subscribe(response => {
        this.messageThreadSource.next([...response, res]);
      });
    });


    this.hubConnection.on("updatedGroup", (group: Group) => {
      console.log("In creat Hub Connection updated Group ");

      if (group.connection.some(x=>x.username == otherUserName))
    {
      this.messageThread$.pipe(take(1)).subscribe(messages => {
        messages.forEach(message => {
          if (message.dateRead) {
            message.dateRead = new Date(Date.now());
          }
        });
        this.messageThreadSource.next([...messages]);
      });
    }
  })
}
stopHubConnection(){
  console.log("In creat Hub Connection  out if stopHubConnection ");

  if (this.hubConnection)
  {
    console.log("In creat Hub Connection in if  stopHubConnection ");

      this.hubConnection.stop();
    }
  }

constructor(private http: HttpClient) { }

getMessages(pageNumber, pageSize, container) {
  let params = getPaginationHeader(pageNumber, pageSize);
  params = params.append('container', container);
  return getPaginatedResult<Message[]>(this.baseUrl + 'Message', params, this.http)
}
getMessagesThread(userName: string) {
  return this.http.get<Message[]>(this.baseUrl + 'Message/thread/' + userName)
}

  async sendMessage(userName: string, content: string) {
    console.log("In message server   sendMessage ");

  //  return this.http.post<Message>(this.baseUrl + 'Message', { recipientUserName: userName, content })
  return this.hubConnection.invoke("sendMessage", { recipientUserName: userName, content })
    .catch(error => console.log(error))
}

deleteMessage(id: number) {
  return this.http.delete(this.baseUrl + 'Message/' + id);
}
}
