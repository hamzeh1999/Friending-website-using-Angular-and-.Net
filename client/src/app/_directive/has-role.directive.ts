import { Directive, Input, OnInit, TemplateRef, ViewContainerRef } from '@angular/core';
import { take } from 'rxjs';
import { User } from '../_models/user';
import { AccountService } from '../_services/account.service';

@Directive({
  selector: '[appHasRole]'
})
export class HasRoleDirective implements OnInit {

  user: User;
  @Input() appHasRole: string[];

  constructor(private viewControllerRef: ViewContainerRef,
    private templateRef: TemplateRef<any>,
    private accountService: AccountService) {
    this.accountService.currentUser$.pipe(take(1)).subscribe(user => {
      this.user = user;
    })
  }
  ngOnInit(): void {

    console.log(this.appHasRole);
    if (!this.user?.roles || this.user == null) {
      this.viewControllerRef.clear();
      return;
    }
    if (this.user?.roles.some(r => this.appHasRole.includes(r))) {
      this.viewControllerRef.createEmbeddedView(this.templateRef);
    }
    else { this.viewControllerRef.clear(); }


    //   throw new Error('Method not implemented.');
  }

}
