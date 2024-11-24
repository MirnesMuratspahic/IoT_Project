import { Component, OnInit, OnDestroy , ViewChild, ElementRef } from '@angular/core';
import { ProfilePageService } from '../profile-page/services/profile-page.service';
import { User } from '../profile-page/models/user.model';
import { AuthService } from '../services/auth.service';
import { NavbarComponent } from '../../core/components/navbar/navbar.component';
import { DeviceResponse } from './models/deviceResponse.model';
import { Device } from './models/device.model';
import { RouterModule } from '@angular/router';
import { CommonModule } from '@angular/common';
import { Subscription, interval } from 'rxjs';
import { FormsModule } from '@angular/forms';
import { write } from 'fs';
import { ConnectDevice } from './models/connectDevice.model';
import { error } from 'console';
import { firstValueFrom } from 'rxjs';
import { catchError } from 'rxjs/operators';
import { of } from 'rxjs';
import { ToastrService } from 'ngx-toastr';


@Component({
  selector: 'app-profile-page',
  standalone: true,
  imports: [CommonModule, NavbarComponent, RouterModule, FormsModule],
  templateUrl: './profile-page.component.html',
  styleUrls: ['./profile-page.component.scss'],
})
export class ProfilePageComponent implements OnInit, OnDestroy {
  user: User | null = null;
  devices: Device[] = [];
  deviceResponses: Map<string, DeviceResponse> = new Map();
  userEmail: string = '';
  deviceId: string = '';
  isGuidValid : number = 1;
  connectDevice: ConnectDevice = {
    deviceId : '',
    userEmail : ''
  };

  private subscription: Subscription | null = null; 

  constructor(
    private profilePageService: ProfilePageService,
    private authService: AuthService,
    private toastrService: ToastrService
  ) {}

  ngOnInit(): void {
    const currentUserEmail = this.authService.getEmailFromToken();

    if (currentUserEmail) {
      this.userEmail = currentUserEmail;
    } else {
      console.error('Email not found in currentUser');
      return;
    }

    this.profilePageService.getUserInformations(this.userEmail).subscribe(
      (data: User) => {
        this.user = data;
      },
      (error) => {
        console.error('Greška prilikom preuzimanja korisničkih podataka:', error);
      }
    );

    this.getDevices();
    this.refreshDeviceResponses();
    this.subscription = interval(5000).subscribe(() => {
      this.refreshDeviceResponses();
    });
  }



  ngOnDestroy(): void {
    if (this.subscription) {
      this.subscription.unsubscribe();
    }
  }

  refreshDeviceResponses() {

    this.devices.forEach((device) => {
      if (device.deviceId) { 
        this.profilePageService.getLastResponse(device.deviceId).pipe(
          catchError((error) => {
            if (error.status === 400) {
              console.log(`Greška pri dohvatanju odgovora za uređaj: ${device.deviceId}`, error);
              return of(null);
            } else {
              console.error('Greška pri dobavljanju podataka za uređaj', error);
              return of(null); 
            }
          })
        ).subscribe(
          (deviceData) => {
            if (deviceData && typeof deviceData === 'object') {
              this.deviceResponses.set(device.deviceId, deviceData); 
            }
          }
        );
      } else {
        console.log(`Ignorisani uređaj sa nevalidnim deviceId: ${device.deviceId}`);
      }
    });
  }
  
  getDevices(): void {
    this.profilePageService.getUserDevices(this.userEmail).subscribe(
      (data: Device[] | string) => {
        if (typeof data === 'object') {
          this.devices = data;
          this.refreshDeviceResponses();
        } else {
          console.error('Unexpected response format:', data);
        }
      },
      (error) => {
        console.error('Greška pri dohvaćanju uređaja:', error);
      }
    );
  }
  

  onInputChange() {
    this.validateGuid();
  }

  validateGuid() {
    const uuidRegex = /^[0-9a-fA-F]{8}-[0-9a-fA-F]{4}-[0-9a-fA-F]{4}-[0-9a-fA-F]{4}-[0-9a-fA-F]{12}$/;
    this.isGuidValid = uuidRegex.test(this.deviceId) ? 2 : 3;
  }

  deleteDevice(deviceId: string) {
    this.profilePageService.deleteDevice(deviceId).subscribe(
      (data: string) => {
          this.getDevices();
          console.log(this.devices);
          this.toastrService.success("Uspješno obrisano!");
      },
      (error) => {
        console.error('Greška pri brisanju uređaja:', error);
        this.toastrService.error("Greška prilikom brisanja!");
      }
    );
  }

  connectNewDevice(deviceId : string){
    this.connectDevice.deviceId = deviceId;
    this.connectDevice.userEmail = this.userEmail;
    console.log(this.connectDevice.userEmail);
    this.profilePageService.connectDevice(this.connectDevice).subscribe(
      (data: string) => {
          console.log(data);
          this.toastrService.success("Uspjesno dodano!");
          this.getDevices();
        },
      (error) => {
        console.log(error);
        this.toastrService.error(error.error);
      }
    )
  }
}
