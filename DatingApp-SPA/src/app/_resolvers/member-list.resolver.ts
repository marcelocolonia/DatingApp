import { Injectable } from '@angular/core';
import { IUser } from '../_models/user';
import {
  Resolve,
  ActivatedRouteSnapshot,
  RouterStateSnapshot,
  Router
} from '@angular/router';
import { UserService } from '../_services/user.service';
import { AlertService } from '../_services/alert.service';
import { Observable, of } from 'rxjs';
import { catchError } from 'rxjs/operators';

@Injectable()
export class MemberListResolver implements Resolve<IUser[]> {
  constructor(
    private userService: UserService,
    private router: Router,
    private alert: AlertService
  ) {}

  resolve(
    route: ActivatedRouteSnapshot,
    state: RouterStateSnapshot
  ): Observable<IUser[]> {

    return this.userService.getAll().pipe(
      catchError(error => {
        this.alert.error(error);
        this.router.navigate(['/home']);
        return of(null);
      })
    );
  }
}
