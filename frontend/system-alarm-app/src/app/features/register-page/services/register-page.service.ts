import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../../environments/environment.development';
import { RegisterData } from '../models/register-data.model';

@Injectable({
  providedIn: 'root',
})
export class RegisterPageService {
  private apiUrl = `${environment.apiBaseUrl}/User/Registration`;

  constructor(private http: HttpClient) {}

  register(userData: RegisterData): Observable<string> {
    return this.http.post(this.apiUrl, userData, { responseType: 'text' });
  }
}
