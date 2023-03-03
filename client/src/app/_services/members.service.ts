import { HttpClient, HttpHandler, HttpHeaders, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from 'enviroments/environment';
import { map, Observable, of, take } from 'rxjs';
import { Memeber } from '../_models/Member';
import { PaginatedResult } from '../_models/Pagination';
import { User } from '../_models/user';
import { UserParms } from '../_models/UserParms';
import { AccountService } from './account.service';
import { getPaginatedResult, getPaginationHeader } from './PaginationHealper';



@Injectable({
  providedIn: 'root'
})
export class MembersService {
  baseUrl = environment.apiUrl;
  member: Memeber[] = [];
  memberCache = new Map();
  user: User;
  userParams: UserParms;
  constructor(private http: HttpClient, private accountService: AccountService) {
    this.accountService.currentUser$.pipe(take(1)).subscribe(user => {
      this.user = user;
    });
    this.userParams = new UserParms(this.user);
  }

  resetUserParams() {
    this.userParams = new UserParms(this.user);
    return this.userParams;
  }
  getUserParams() {
    return this.userParams;
  }
  setUserParams(params: UserParms) {
    this.userParams = params;
  }
 
  getMembers(userParms: UserParms) {

    var response = this.memberCache.get(Object.values(userParms).join("-"));
    if (response) {
      return of(response)
    }
    let parms = getPaginationHeader(userParms.pageNumber, userParms.pageSize);
    parms = parms.append("minAge", userParms.minAge.toString());
    parms = parms.append("maxAge", userParms.maxAge.toString());
    parms = parms.append("gender", userParms.gender);
    parms = parms.append("orderBy", userParms.orderBy);


    return getPaginatedResult<Memeber[]>(this.baseUrl + 'Users', parms,this.http).pipe(map(res => {
      this.memberCache.set(Object.values(userParms).join("-"), res);
      return res;
    }));


  }




  getMember(userName: string) {
    console.log(this.memberCache);
    const member = [...this.memberCache.values()]
      .reduce((arr, elem) => arr.concat(elem.result), [])
      .find((member: Memeber) => member.userName == userName);
    if (member)
      return of(member);
    console.log(member);
    return this.http.get<Memeber>(this.baseUrl + 'Users/' + userName);

  }

  updateMember(member: Memeber) {
    return this.http.put(this.baseUrl + 'Users/', member).pipe(
      map(() => {
        const index = this.member.indexOf(member);
        this.member[index] = member;
      })
    );

  }

  setMainPhoto(photoId: number) {
    return this.http.put(this.baseUrl + 'Users/set-main-photo/' + photoId, {});
  }

  deletePhoto(photoId: number) {
    return this.http.delete(this.baseUrl + 'Users/delete-photo/' + photoId)
  }



  addLike(userName: string) {
    return this.http.post(this.baseUrl + "Likes/" + userName, {});
  }

  getLikes(predict: string, pageNumber, pageSize) {
    let params = getPaginationHeader(pageNumber, pageSize)
    params = params.append('predict', predict);
    return getPaginatedResult<Partial<Memeber[]>>(this.baseUrl+'likes',params,this.http); //for pagination 
   // return this.http.get<Partial<Memeber[]>>(this.baseUrl + "Likes?predict=" + predict);
  }




}
