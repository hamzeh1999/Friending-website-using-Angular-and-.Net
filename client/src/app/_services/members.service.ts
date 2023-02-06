import { HttpClient, HttpHandler, HttpHeaders, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from 'enviroments/environment';
import { map, Observable, of } from 'rxjs';
import { Memeber } from '../_models/Member';
import { PaginatedResult } from '../_models/Pagination';
import { UserParms } from '../_models/UserParms';



@Injectable({
  providedIn: 'root'
})
export class MembersService {
  baseUrl = environment.apiUrl;
  member: Memeber[] = [];
  constructor(private http: HttpClient) { }

  private getPaginationHeader(pageNumber: number, pageSize: number) {
    let parms = new HttpParams();
    parms = parms.append("pageNumber", pageNumber.toString());
    parms = parms.append("pageSize", pageSize.toString());
    
    return parms;

  }
  private getPaginatedResult<T>(url, params) {
    const paginatedResult: PaginatedResult<T> = new PaginatedResult<T>();


    return this.http.get<T>(url, { observe: 'response', params }).pipe(
      map(response => {
        paginatedResult.result = response.body;
        if (response.headers.get('Pagination') !== null)
          paginatedResult.pagination = JSON.parse(response.headers.get('Pagination'));

          return paginatedResult; 
      }
      ));

  }
  getMembers(userParms: UserParms) {
    console.log(Object.values(userParms).join("-"));
    let parms = this.getPaginationHeader(userParms.pageNumber, userParms.pageSize);
    parms = parms.append("minAge", userParms.minAge.toString());
    parms = parms.append("maxAge", userParms.maxAge.toString());
    parms = parms.append("gender", userParms.gender);
    parms = parms.append("orderBy", userParms.orderBy);


    return this.getPaginatedResult<Memeber[]>(this.baseUrl + 'Users', parms);


  }

  getMember(userName: string) {
    const member = this.member.find(x => x.userName === userName);
    if (member !== undefined) return of(member);
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

}
