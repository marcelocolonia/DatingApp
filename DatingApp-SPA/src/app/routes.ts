import { Routes } from '@angular/router';
import { HomeComponent } from './home/home.component';
import { MemberListComponent } from './member-list/member-list.component';
import { ListsComponent } from './lists/lists.component';
import { MessagesComponent } from './messages/messages.component';

export const appRoutes: Routes = [
  {
    path: 'home',
    component: HomeComponent
  },
  {
    path: 'members',
    component: MemberListComponent
  },
  {
      path: 'lists',
      component: ListsComponent
  },
  {
      path: 'messages',
      component: MessagesComponent
  },
  { path: '**', redirectTo: 'home', pathMatch: 'full' }
];
