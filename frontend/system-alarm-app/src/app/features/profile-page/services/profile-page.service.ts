import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { User } from '../models/user.model';
import { environment } from '../../../environments/environment.development';
import { ConnectDevice } from '../models/connectDevice.model';
import { DeviceResponse } from '../models/deviceResponse.model';
import { Device } from '../models/device.model';

@Injectable({
  providedIn: 'root',
})
export class ProfilePageService {
  private getUserInfoUrl = `${environment.apiBaseUrl}/User/GetUserInformations`;
  private getUserDevicesUrl = `${environment.apiBaseUrl}/Device/GetUserDevices`;
  private connectDeviceUrl = `${environment.apiBaseUrl}/Device/ConnectDevice`;
  private reciveResponseUrl = `${environment.apiBaseUrl}/Device/GetLastResponse`;

  constructor(private http: HttpClient) {}

  getUserInformations(email: string): Observable<User> {
    const headers = { 'Content-Type': 'application/json' };
    return this.http.post<User>(this.getUserInfoUrl, JSON.stringify(email), {headers});
  };

  getUserDevices(email: string): Observable<Device[]> {
    const headers = { 'Content-Type': 'application/json' };
    return this.http.post<Device[]>(this.getUserDevicesUrl, JSON.stringify(email), {headers});
  }

  connectDevice(connectDevice: ConnectDevice): Observable<string> {
    const headers = { 'Content-Type': 'application/json' };
    return this.http.post<string>(this.connectDeviceUrl, connectDevice, {headers});
  };

  getLastResponse(deviceId: string) : Observable<DeviceResponse> {
    const headers = { 'Content-Type': 'application/json' };
    return this.http.post<DeviceResponse>(this.reciveResponseUrl, JSON.stringify(deviceId), {headers});
  };

  
}
