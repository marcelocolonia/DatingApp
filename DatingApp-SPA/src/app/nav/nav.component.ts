import { Component, OnInit } from '@angular/core';
import { AuthService } from '../_services/auth.service';
import { AlertService } from '../_services/alert.service';
import { Router } from '@angular/router';

@Component({
  selector: 'app-nav',
  templateUrl: './nav.component.html',
  styleUrls: ['./nav.component.css']
})
export class NavComponent implements OnInit {
  model: any = {};

  constructor(
    private authService: AuthService,
    private alert: AlertService,
    private router: Router
  ) {}

  get userName(): string {
    return this.authService.getUserName();
  }

  ngOnInit(): void {}

  login(): void {
    this.authService.login(this.model).subscribe(
      next => {
        this.alert.success('Logged in successfully');
      },
      error => {
        this.alert.error(error);
      },
      () => {
        this.router.navigate(['/members']);
      }
    );
  }

  get loggedIn(): boolean {
    return this.authService.loggedIn();
  }

  logout(): void {
    this.router.navigate(['/home']).then(() => {
      this.authService.logout();
    });
  }
}
