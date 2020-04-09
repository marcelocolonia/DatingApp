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
export class MemberDetailResolver implements Resolve<IUser> {
  constructor(
    private userService: UserService,
    private router: Router,
    private alert: AlertService
  ) {}

  resolve(
    route: ActivatedRouteSnapshot,
    state: RouterStateSnapshot
  ): Observable<IUser> {
    const userId = (route.params as IUser).id;

    return this.userService.get(userId).pipe(
      catchError(error => {
        this.alert.error(error);
        this.router.navigate(['/members']);
        return of(null);
      })
    );
  }
}
