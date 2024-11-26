import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable, throwError } from 'rxjs';
import { catchError, map } from 'rxjs/operators';
import { environment } from '../../../environments/environment.development';
import { dtoUserCode } from '../models/dtoUserCode.model';


@Injectable({
  providedIn: 'root'
})
export class CodePageService {

  constructor(private http: HttpClient) {}

  sendCodeUrl = `${environment.apiBaseUrl}/User/AcceptUserCode`;

  sendCodeToVerifyEmail(dtoUserCode: dtoUserCode) : Observable<string> {
    const headers = { 'Content-Type': 'application/json' };
    return this.http.post<string>(this.sendCodeUrl, dtoUserCode, {headers} );
  }

  
}
