import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { AdminPanelComponent } from './admin/admin-panel/admin-panel.component';
import { NotFoundComponent } from './errors/not-found/not-found.component';
import { ServerErrorComponent } from './errors/server-error/server-error.component';
import { TestErrorComponent } from './errors/test-error/test-error.component';
import { HomeComponent } from './home/home.component';
import { ListComponent } from './list/list.component';
import { MemberDetailComponent } from './members/member-detail/member-detail.component';
import { MemberEditComponent } from './members/member-edit/member-edit.component';
import { MemberListComponent } from './members/member-list/member-list.component';
import { MessagesComponent } from './messages/messages.component';
import { AdminGuard } from './_guards/admin.guard';
import { AuthGuard } from './_guards/auth.guard';
import { PreventUnsavedChangesGuard } from './_guards/prevent-unsaved-changes.guard';
import { MemberDetailResolver } from './_resolvers/member-details.resolver';

const routes: Routes = [
  { path: '', component: HomeComponent },
  { path: 'not-found', component: NotFoundComponent },
  { path: 'server-error', component: ServerErrorComponent },
  { path: 'errors', component: TestErrorComponent },
  {
    path: '',
    runGuardsAndResolvers: 'always',
    canActivate: [AuthGuard],
    children: [
      { path: 'Admin', component:AdminPanelComponent,canActivate:[AdminGuard] },
      { path: 'lists', component: ListComponent },
      { path: 'messages', component: MessagesComponent },
      { path: 'members', component: MemberListComponent },
      { path: 'member/edit', component: MemberEditComponent,canDeactivate:[PreventUnsavedChangesGuard ] },
      { path: 'members/:userName', component: MemberDetailComponent,resolve:{'member':MemberDetailResolver} },

    ]
  },
  { path: '**', pathMatch: "full", component: HomeComponent },





];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
