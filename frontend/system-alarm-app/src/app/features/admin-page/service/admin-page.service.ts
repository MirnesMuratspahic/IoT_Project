import { Injectable } from '@angular/core';
import { HttpClient, HttpErrorResponse, HttpHeaders } from '@angular/common/http';
import { environment } from '../../../environments/environment';
import { catchError, Observable } from 'rxjs';
import { Device } from '../../profile-page/models/device.model';
import { throwError } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class AdminPageService {

  constructor(private http: HttpClient) { }

  createHeaders(): HttpHeaders {
    const token = localStorage.getItem('token');
    return new HttpHeaders({
      'Authorization': `Bearer ${token}`,
      'Content-Type': 'application/json'
    });
  }

  private handleError(error: HttpErrorResponse) {
    if (error.status === 400) {
      return throwError(() => ({ status: error.status, message: error.error.name || 'Došlo je do greške!' }));
    } else {
      return throwError(() => ({ status: error.status, message: 'Neuspešan poziv!' }));
    }
  }

  deleteDevice(deviceId: string) : Observable<string> {
    const headers = this.createHeaders();
    const createDevicesUrl = `${environment.apiBaseUrl}/Device/DeleteDeviceAdmin/${deviceId}`; 
    return this.http.delete<string>(createDevicesUrl, {headers} ).pipe(catchError(this.handleError));
  }

  addDevice(deviceCount: number) : Observable<Device[]> {
    const headers = this.createHeaders();
    const createDevicesUrl = `${environment.apiBaseUrl}/Device/AddDevice/${deviceCount}`; 
    return this.http.post<Device[]>(createDevicesUrl, {}, {headers} ).pipe(catchError(this.handleError));
  }

  getDevices() : Observable<Device[]> {
    const headers = this.createHeaders();
    const getDevicesUrl = `${environment.apiBaseUrl}/Device/GetDevices`; 
    return this.http.get<Device[]>(getDevicesUrl, {headers}).pipe(catchError(this.handleError));
  }
  
}
