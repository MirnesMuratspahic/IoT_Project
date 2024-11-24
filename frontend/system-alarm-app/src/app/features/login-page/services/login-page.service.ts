import { HttpClient } from '@angular/common/http';
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

  login(data: LoginData): Observable<LoginResponse> {
    return this.http.post(this.apiUrl, data, { responseType: 'text' }).pipe(
      map((response: string) => {
        return { token: response } as LoginResponse;
      }),
      catchError((error) => {
        console.error('Login error:', error);
        return throwError(() => new Error('Login failed'));
      })
    );
  }
}
