import { Injectable } from '@angular/core';
import { HttpClient, HttpErrorResponse, HttpHeaders  } from '@angular/common/http';
import { User } from '../models/user.model';
import { environment } from '../../../environments/environment.development';
import { ConnectDevice } from '../models/connectDevice.model';
import { DeviceResponse } from '../models/deviceResponse.model';
import { Device } from '../models/device.model';
import { response } from 'express';
import { map } from 'rxjs/operators';
import { Observable, throwError } from 'rxjs';
import { catchError } from 'rxjs/operators';

@Injectable({
  providedIn: 'root',
})
export class ProfilePageService {
  private getUserInfoUrl = `${environment.apiBaseUrl}/User/GetUserInformations`;
  private getUserDevicesUrl = `${environment.apiBaseUrl}/Device/GetUserDevices`;
  private connectDeviceUrl = `${environment.apiBaseUrl}/Device/ConnectDevice`;
  private reciveResponseUrl = `${environment.apiBaseUrl}/Device/GetLastResponse`;
  private deleteDeviceUrl = `${environment.apiBaseUrl}/Device/DeleteDevice`;

  constructor(private http: HttpClient) {}

  createHeaders(): HttpHeaders {
    const token = localStorage.getItem('token');
    return new HttpHeaders({
      'Authorization': `Bearer ${token}`,
      'Content-Type': 'application/json'
    });
  }

  deleteDevice(deviceId: string): Observable<string> {
    const headers = this.createHeaders();
    const url = `${this.deleteDeviceUrl}/${deviceId}`;
    return this.http.delete<string>(url, { headers});
  }

  getUserInformations(email: string): Observable<User> {
    const headers = this.createHeaders();
    return this.http.post<User>(this.getUserInfoUrl, JSON.stringify(email), {headers}).pipe(catchError(this.handleError));
  };

  getUserDevices(email: string): Observable<Device[] | string> {
    const headers = this.createHeaders();
    return this.http.post<Device[] | string>(this.getUserDevicesUrl, JSON.stringify(email), {headers}).pipe(catchError(this.handleError));;
  }

  connectDevice(connectDevice: ConnectDevice): Observable<string> {
    const headers = this.createHeaders();
    return this.http.post<string>(this.connectDeviceUrl, connectDevice, {headers}).pipe(catchError(this.handleError));
  };

  getLastResponse(deviceId: string): Observable<DeviceResponse | string> {
    const headers = this.createHeaders();
    return this.http.post(this.reciveResponseUrl, JSON.stringify(deviceId), { headers, responseType: 'text' })
      .pipe(
        map(response => {
          try {
            return JSON.parse(response) as DeviceResponse;
          } catch {
            return response;
          }
        })
      );
  }

  private handleError(error: HttpErrorResponse) {
    if (error.status === 400) {
      return throwError(() => ({ status: error.status, message: error.error.name || 'Error occured!'}));
    } else {
      return throwError(() => ({ status: error.status, message: 'Error occured!' }));
    }
  }

  
}
  

  

