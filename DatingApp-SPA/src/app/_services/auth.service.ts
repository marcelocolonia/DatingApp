import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { map } from 'rxjs/operators';
import { Observable } from 'rxjs';
import { TokenStorageService } from './token-storage.service';

@Injectable({
  providedIn: 'root'
})
export class AuthService {

  baseUrl = 'http://localhost:5000/api/auth/';

  constructor(private http: HttpClient, private tokenStorage: TokenStorageService) { }

  login(model: any): Observable<any> {
    return this.http.post(this.baseUrl + 'login', model)
      .pipe(
        map((response: any) => {
          if (response) {
            this.tokenStorage.storeToken(response.token);
          }
        })
      );
  }

  register(model: any): Observable<any> {
    return this.http.post(this.baseUrl + 'register', model);
  }

  loggedIn(): boolean {
    return !this.tokenStorage.isTokenExpired();
  }

  logout(): void {
    this.tokenStorage.clearToken();
  }

  getUserName(): any {
    return this.tokenStorage.getUserName();
  }
}
