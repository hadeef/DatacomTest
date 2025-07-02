import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { map } from 'rxjs/operators';

export interface Application {
  id: number;
  companyName: string;
  position: string;
  status: number;
  dateApplied: string;
  createdAt: string;
  updatedAt: string;
}

@Injectable({
  providedIn: 'root'
})
export class ApplicationService {
  //Use relative API URL in Production with uncommenting the line below and set port in environment settings.
  //private apiUrl = '/api/Application';
  private apiUrl = 'https://localhost:7069/api/Application';

  constructor(private http: HttpClient) { }

  getAllApplications(): Observable<Application[]> {
    return this.http.get<Application[]>(this.apiUrl);
  }

  addApplication(app: Application): Observable<any> {
    return this.http.post<Application>(this.apiUrl, app, { observe: 'response' });
  }

  updateApplication(app: Application): Observable<any> {
    return this.http.put<Application>(this.apiUrl, app, { observe: 'response' });
  }
}
