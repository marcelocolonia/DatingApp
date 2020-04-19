import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { environment } from 'src/environments/environment';
import { HttpClient } from '@angular/common/http';

@Injectable({
  providedIn: 'root'
})
export abstract class ApiService<T> {

  get Resource(): string {
    return environment.apiUrl + this.subResource;
  }

  public abstract subResource: string;

  constructor(protected httpClient: HttpClient) {
  }

  getAll(): Observable<T[]> {
    return this.httpClient.get<T[]>(this.Resource);
  }

  get(id: number): Observable<T> {
    return this.httpClient.get<T>(this.Resource + '/' + id);
  }

  update(model: T): Observable<any> {
    return this.httpClient.post<T>(this.Resource + '/update', model);
  }
}
