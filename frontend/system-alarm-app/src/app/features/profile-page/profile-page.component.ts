import { Component, OnInit, OnDestroy } from '@angular/core';
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

  private subscription: Subscription | null = null; 

  constructor(
    private profilePageService: ProfilePageService,
    private authService: AuthService
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

  refreshDeviceResponses(): void {
    this.profilePageService.getUserDevices(this.userEmail).subscribe(
      (data: Device[]) => {
        this.devices = data;

        this.devices.forEach((device) => {
          this.profilePageService.getLastResponse(device.deviceId).subscribe(
            (deviceData) => {
              this.deviceResponses.set(device.deviceId, deviceData);
            },
            (error) => {
              console.error(
                `Greška pri dohvaćanju podataka za uređaj ${device.deviceId}:`,
                error
              );
            }
          );
        });
      },
      (error) => {
        console.error('Greška pri dohvaćanju uređaja:', error);
      }
    );
  }
}
