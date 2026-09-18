import {Injectable} from '@angular/core';
import {HttpClient} from '@angular/common/http';
import {Observable} from 'rxjs';
import {JobApplication, CreateJobApplicationRequest, UpdateJobApplicationRequest } from '../models/job-application.model';

@Injectable({providedIn: 'root'})
export class JobApplicationService {

  private readonly apiUrl = 'http://localhost:5247/api/jobapplications';


  constructor(private http: HttpClient) {
  }

  getAll(): Observable<JobApplication[]> {
    return this.http.get<JobApplication[]>(this.apiUrl);
  }

  getById(id: number): Observable<JobApplication> {
    return this.http.get<JobApplication>(`${this.apiUrl}/${id}`);
  }

  create(request: CreateJobApplicationRequest):
    Observable<JobApplication> {
    return this.http.post<JobApplication>(`${this.apiUrl}`, request);
  }

  update(id: number, request: UpdateJobApplicationRequest): Observable<void> {
    return this.http.put<void>(`${this.apiUrl}/${id}`, request);
  }

  delete(id: number): Observable<void> {
    return this.http.delete<void>(`${this.apiUrl}/${id}`);
  }
}
