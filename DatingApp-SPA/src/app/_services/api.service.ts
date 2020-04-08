import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { environment } from 'src/environments/environment';
import { TokenStorageService } from './token-storage.service';
import { HttpClient, HttpHeaders } from '@angular/common/http';

@Injectable({
  providedIn: 'root'
})
export abstract class ApiService<T> {

  private readonly httpOptions = {};

  get Resource(): string {
    return environment.apiUrl + this.SubResource;
  }

  public abstract SubResource: string;

  constructor(private httpClient: HttpClient, tokenStorage: TokenStorageService) {

    this.httpOptions = {
      headers: new HttpHeaders({
        Authorization: 'Bearer ' + tokenStorage.getToken()
      })
    };

  }

  getAll(): Observable<T[]> {
    return this.httpClient.get<T[]>(this.Resource, this.httpOptions);
  }

  get(id: number): Observable<T> {
    return this.httpClient.get<T>(this.Resource + '/' + id, this.httpOptions);
  }
}
