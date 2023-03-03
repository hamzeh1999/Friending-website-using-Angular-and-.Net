import { Injectable } from '@angular/core';
import { ActivatedRouteSnapshot, CanDeactivate, RouterStateSnapshot, UrlTree } from '@angular/router';
import { Observable } from 'rxjs';
import { MemberEditComponent } from '../members/member-edit/member-edit.component';
import { ConfirmService } from '../_services/confirm.service';

@Injectable({
  providedIn: 'root'
})
export class PreventUnsavedChangesGuard implements CanDeactivate<unknown> {

  constructor(private confirmService:ConfirmService){}
  canDeactivate(
    component: MemberEditComponent,):boolean|Observable<boolean>{
      if(component.editForm.dirty){
// return confirm("Are You sure want to Continue? Any Unsaved changes will be lost ")
return this.confirmService.confirm( )

}
    return true;
  }
  
}
