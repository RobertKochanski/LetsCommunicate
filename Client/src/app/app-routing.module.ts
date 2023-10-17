import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { HomeComponent } from './home/home.component';
import { AuthGuard } from './_guards/auth.guard';
import { ServerErrorComponent } from './errors/server-error/server-error.component';
import { NotFoundComponent } from './errors/not-found/not-found.component';
import { FriendsComponent } from './friends/friends.component';
import { InvitationsComponent } from './invitations/invitations.component';
import { ProfileComponent } from './profile/profile.component';
import { EditComponent } from './profile/edit/edit.component';
import { ChatComponent } from './chat/chat.component';
import { MembersComponent } from './chat/members/members.component';

const routes: Routes = [
  {path: '', component: HomeComponent, title: "Let's Communicate"},
  {
    path: '',
    title: "Let's Communicate",
    runGuardsAndResolvers: 'always',
    canActivate: [AuthGuard],
    children: [
      {path: 'chat', component: ChatComponent},
      {path: 'chat/:id', component: ChatComponent},
      {path: 'chat/:id/members', component: MembersComponent},

      {path: 'friends', component: FriendsComponent},
      {path: 'invitations', component: InvitationsComponent},
      {path: 'profile', component: ProfileComponent},
      {path: 'profile/edit', component: EditComponent},
    ]
  },
  {path: 'server-error', component: ServerErrorComponent, title: "Server-error"},
  {path: '**', component: NotFoundComponent, pathMatch: 'full', title: "Not-found"},
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
