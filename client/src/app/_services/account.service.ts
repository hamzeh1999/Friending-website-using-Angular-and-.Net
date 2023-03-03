import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from 'enviroments/environment';
import { BehaviorSubject, map } from 'rxjs';
import { User } from '../_models/user';
import { PresenceService } from './presence.service';

@Injectable({
  providedIn: 'root'
})
export class AccountService {
  baseUrl = environment.apiUrl;
  private currentUserSource = new BehaviorSubject<User | null>(null);
  currentUser$ = this.currentUserSource.asObservable();

  constructor(private http: HttpClient,private presence:PresenceService) { }

  login(model: any) {
    return this.http.post<User>(this.baseUrl + 'AccountControler/login', model).pipe(
      map((response: User) => {
        console.log("In loging account photo :" + response.photoUrl);
        const user = response;
        if (user) {
          this.setCurrentUser(user);
          this.presence.createHubConnection(user);
        }
      })
    )
  }

  register(model: any) {
    return this.http.post<User>(this.baseUrl + 'AccountControler/register', model).pipe(
      map(user => {
        if (user) {
          this.setCurrentUser(user);
          this.presence.createHubConnection(user);

        }
      })
    )
  }
  
  setCurrentUser(user: User) {
    user.roles=[];
    const role=this.getDecodedToken(user.token).role
    Array.isArray(role)?user.roles=role:user.roles.push(role);
    localStorage.setItem('user', JSON.stringify(user));
    this.currentUserSource.next(user);
  }

  logout() {
    localStorage.removeItem('user');
    this.currentUserSource.next(null);
    this.presence.stopHubConnection();
  }

 getDecodedToken(token:string){
return JSON.parse(atob(token.split(".")[1]))
 }
}
