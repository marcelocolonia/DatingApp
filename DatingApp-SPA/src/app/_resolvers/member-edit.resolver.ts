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
import { TokenStorageService } from '../_services/token-storage.service';

@Injectable()
export class MemberEditResolver implements Resolve<IUser> {
  constructor(
    private userService: UserService,
    private router: Router,
    private alert: AlertService,
    private tokenService: TokenStorageService,
  ) {}

  resolve(
    route: ActivatedRouteSnapshot,
    state: RouterStateSnapshot
  ): Observable<IUser> {

    const userId = this.tokenService.getUserId();

    return this.userService.get(userId).pipe(
      catchError(error => {
        this.alert.error(error);
        this.router.navigate(['/members']);
        return of(null);
      })
    );
  }
}
