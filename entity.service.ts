import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';

@Injectable({
  providedIn: 'root'
})
export class EntityService {

  apiUrl = 'http://localhost:5019/api/Entity';

  constructor(private http: HttpClient) { }

  getEntities() {
    return this.http.get(this.apiUrl);
  }

  addEntity(entity: any) {
    return this.http.post(this.apiUrl, entity);
  }

}