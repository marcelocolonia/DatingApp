import { Injectable } from '@angular/core';
import { ApiService } from './api.service';
import { IUser } from '../_models/user';

@Injectable({
  providedIn: 'root'
})
export class UserService extends ApiService<IUser> {

  public subResource = 'users';

}
