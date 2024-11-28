import { HttpClient, HttpErrorResponse  } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable, throwError } from 'rxjs';
import { catchError, map } from 'rxjs/operators';
import { environment } from '../../../environments/environment.development';
import { LoginResponse } from '../models/login-response.model';
import { LoginData } from '../models/login-data.model';

@Injectable({
  providedIn: 'root',
})
export class LoginPageService {
  private apiUrl = `${environment.apiBaseUrl}/User/Login`;

  constructor(private http: HttpClient) {}

  login(data: LoginData): Observable<string> {
    const headers = { 'Content-Type': 'application/json' };
    return this.http.post<string>(this.apiUrl, data, {headers, responseType: 'text' as 'json'} ).pipe(
      catchError(this.handleError));
  }

  private handleError(error: HttpErrorResponse) {
    if (error.status === 400 && error.error && error.error.name) {
      console.error('Login error:', error.error.name);
      return throwError(() => new Error(error.error.name));
    } else {
      return throwError(() => new Error(error.error.name));
    }
  }
}
