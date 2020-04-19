import { Injectable } from '@angular/core';
import { ApiService } from './api.service';
import { Photo } from '../_models/photo';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class PhotoService extends ApiService<Photo> {

  public subResource = 'photos';

  setMain(id: number): Observable<any>  {
    return this.httpClient.post<Photo>(`${this.Resource}/${id}/setMain`, {});
  }

}
