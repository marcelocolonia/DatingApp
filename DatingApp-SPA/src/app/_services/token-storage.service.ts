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
    const token = localStorage.getItem('token');
    return this.jwtHelper.isTokenExpired(token);
  }

  clearToken(): void {
    localStorage.removeItem('token');
  }

  getUserName(): string {
    const token = localStorage.getItem('token');
    const data = this.jwtHelper.decodeToken(token);
    return data.nameid['1'];
  }
}
