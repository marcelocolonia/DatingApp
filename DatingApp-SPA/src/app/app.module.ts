import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { HttpClientModule } from '@angular/common/http';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { RouterModule } from '@angular/router';
import { BsDropdownModule } from 'ngx-bootstrap';
import { JwtModule } from '@auth0/angular-jwt';

import { AppComponent } from './app.component';
import { NavComponent } from './nav/nav.component';
import { AuthService } from './_services/auth.service';
import { HomeComponent } from './home/home.component';
import { RegisterComponent } from './register/register.component';
import { ErrorInterceptorProvider } from './_services/error.interceptor';
import { AlertService } from './_services/alert.service';
import { TokenStorageService, tokenGetter } from './_services/token-storage.service';
import { MemberListComponent } from './members/member-list/member-list.component';
import { appRoutes } from './routes';
import { ListsComponent } from './lists/lists.component';
import { MessagesComponent } from './messages/messages.component';
import { UserService } from './_services/user.service';
import { MemberCardComponent } from './members/member-card/member-card.component';
import { environment } from 'src/environments/environment';
import { MemberDetailComponent } from './members/member-detail/member-detail.component';

@NgModule({
  declarations: [
    AppComponent,
    NavComponent,
    HomeComponent,
    RegisterComponent,
    MemberListComponent,
    MemberCardComponent,
    ListsComponent,
    MessagesComponent,
    MemberDetailComponent,
  ],
  imports: [
    BrowserModule,
    HttpClientModule,
    FormsModule,
    BrowserAnimationsModule,
    BsDropdownModule.forRoot(),
    RouterModule.forRoot(appRoutes),
    JwtModule.forRoot({
      config: {
        tokenGetter,
        whitelistedDomains: [environment.domain],
        blacklistedRoutes: [environment.apiUrl + 'auth']
      }
    })
  ],
  providers: [
    AuthService,
    TokenStorageService,
    ErrorInterceptorProvider,
    AlertService,
    UserService,
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
