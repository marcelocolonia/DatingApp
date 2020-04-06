import { Component, OnInit } from '@angular/core';
import { AuthService } from '../services/auth.service';
import { AlertService } from '../services/alert.service';

@Component({
  selector: 'app-nav',
  templateUrl: './nav.component.html',
  styleUrls: ['./nav.component.css']
})
export class NavComponent implements OnInit {

  model: any = {};

  constructor(private authService: AuthService, private alert: AlertService) {}

  get userName(): string {
    return this.authService.getUserName();
  }

  ngOnInit(): void {}

  login(): void {
    this.authService.login(this.model).subscribe(
      response => {
        this.alert.success('Logged in successfully');
      },
      error => {
        this.alert.error(error);
      }
    );
  }

  get loggedIn(): boolean {
    return this.authService.loggedIn();
  }

  logout(): void {
    this.authService.logout();
    this.alert.message('Logged out');
  }
}
