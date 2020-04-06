import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class ApiService<T> {

  private resource = 'http://localhost:5000/api/';

  constructor(private httpClient: HttpClient) {
  }

  getAll(x: new () => T): Observable<T[]> {
    return this.httpClient.get(this.resource + x.name) as any;
  }
}
