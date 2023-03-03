import { Injectable } from "@angular/core";
import { ActivatedRouteSnapshot, Resolve, RouterStateSnapshot } from "@angular/router";
import { Observable } from "rxjs";
import { Memeber } from "../_models/Member";
import { MembersService } from "../_services/members.service";


@Injectable({
    providedIn:'root'
})

export class MemberDetailResolver implements Resolve<Memeber>
{
    constructor(private memberService:MembersService){}
    resolve(route: ActivatedRouteSnapshot): Observable<Memeber>
    {
    return this.memberService.getMember(route.paramMap.get('userName'));
    
    }
}
