import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { map } from 'rxjs/operators';
import { Observable } from 'rxjs';
import { TokenStorageService } from './token-storage.service';
import { environment } from 'src/environments/environment';

@Injectable({
  providedIn: 'root'
})
export class AuthService {

  constructor(private http: HttpClient, private tokenStorage: TokenStorageService) { }

  login(model: any): Observable<any> {
    return this.http.post(environment.apiUrl + 'auth/login', model)
      .pipe(
        map((response: any) => {
          if (response) {
            this.tokenStorage.storeToken(response.token);
          }
        })
      );
  }

  register(model: any): Observable<any> {
    return this.http.post(environment.apiUrl + 'auth/register', model);
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
