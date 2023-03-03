import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from 'enviroments/environment';
import { User } from '../_models/user';

@Injectable({
  providedIn: 'root'
})
export class AdminService {
baseUrl=environment.apiUrl;

  constructor(private http:HttpClient) { }
  getUSerWithRoles(){
    return this.http.get<Partial<User[]>>(this.baseUrl+'admin/user-with-Roles')
  }


  updateUsrRole(userName:string,roles:string[]){
    return this.http.post(this.baseUrl+'admin/edit-roles/'+userName+'?roles='+roles,{});
  }
}
