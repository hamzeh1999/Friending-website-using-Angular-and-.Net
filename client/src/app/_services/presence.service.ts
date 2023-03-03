import { Injectable } from '@angular/core';
import { Router } from '@angular/router';
import { HubConnection } from '@microsoft/signalr';
import { HubConnectionBuilder } from '@microsoft/signalr/dist/esm/HubConnectionBuilder';
import { environment } from 'enviroments/environment';
import { ToastrService } from 'ngx-toastr';
import { BehaviorSubject, take } from 'rxjs';
import { User } from '../_models/user';

@Injectable({
  providedIn: 'root'
})
export class PresenceService {
hubUrl=environment.hubUrl;
private hubConnection:HubConnection;
private onlineUserSource=new BehaviorSubject<string[]>([]);
onlineUser$=this.onlineUserSource.asObservable();


  constructor(private toastr:ToastrService,private router:Router ) { }

  createHubConnection(user:User){
    this.hubConnection=new HubConnectionBuilder()
    .withUrl(this.hubUrl+'presence',{
      accessTokenFactory:()=>user.token
    })
    .withAutomaticReconnect()
    .build();
    this.hubConnection.start().catch(error=>console.log("In Create Connection Error❌ :"+error));
   
    this.hubConnection.on("UserIsOnline",username=>{
      //this.toastr.info(username + ' has connected');
      this.onlineUser$.pipe(take(1)).subscribe(usernames=>{
        console.log("In UserIsOnlineUser usernames Fisrt:"+usernames);
        console.log("In UserIsOnlineUser username Second:"+username);
        
        this.onlineUserSource.next([...usernames,username])
      })
   
    });


    this.hubConnection.on("UserIsOffline",username=>{
      //this.toastr.warning(username+' has disconnected')
    this.onlineUser$.pipe(take(1)).subscribe(usernames=>{
      console.log("In UserIsOffline usernames Fisrt:"+usernames);
      console.log("In UserIsOffline username Second:"+username);
      this.onlineUserSource.next([...usernames.filter(x=>x!==username)]);
    })
    });

    this.hubConnection.on('GetOnlineUsers',(username:string[])=>{
      this.onlineUserSource.next(username)
    });


    this.hubConnection.on('NewMessageReceived',({username,knownAs})=>{
      this.toastr.info(knownAs+" has sent a new \n message📨 ")
      .onTap
      .pipe(take(1))
      .subscribe(()=>this.router.navigateByUrl('/members/'+username+'?tab=3'));

    });
   
  }
  stopHubConnection(){
    this.hubConnection.stop().catch(error=>console.log("In Stop Connection Error❌ :"+error));
  }
}
