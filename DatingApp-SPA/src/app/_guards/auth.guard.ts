import { Injectable } from '@angular/core';
import { CanActivate, Router } from '@angular/router';
import { AuthService } from '../_services/auth.service';
import { AlertService } from '../_services/alert.service';

@Injectable({
  providedIn: 'root'
})
export class AuthGuard implements CanActivate {
  constructor(
    private authService: AuthService,
    private router: Router,
    private alertService: AlertService
  ) {}

  canActivate(): boolean {
    if (this.authService.loggedIn()) {
      return true;
    }

    if (!this.authService.loggedIn()) {
      this.router
        .navigate(['/home'])
        .then(() => {
          this.alertService.error('You need to log in');
        })
        .finally(() => {
          return false;
        });
    }
  }
}
