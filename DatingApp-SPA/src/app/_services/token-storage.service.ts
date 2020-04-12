import { Injectable } from '@angular/core';
import { JwtHelperService } from '@auth0/angular-jwt';

export function tokenGetter() {
  return localStorage.getItem('token');
}

@Injectable({
  providedIn: 'root'
})
export class TokenStorageService {

  jwtHelper = new JwtHelperService();

  constructor() { }

  storeToken(token: string): void {
    localStorage.setItem('token', token);
  }

  isTokenExpired(): boolean {
    return this.jwtHelper.isTokenExpired(tokenGetter());
  }

  clearToken(): void {
    localStorage.removeItem('token');
  }

  getUserName(): string {
    const data = this.jwtHelper.decodeToken(tokenGetter());
    return data.nameid[1];
  }

  getUserId(): number {
    const data = this.jwtHelper.decodeToken(tokenGetter());
    return data.nameid[0];
  }
}
