import { Component, OnInit } from '@angular/core';
import { NavbarComponentAdmin } from '../../core/components/admin-navbar/navbar.component';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { AdminPageService } from './service/admin-page.service';
import { Device } from '../profile-page/models/device.model';
import { ToastrService } from 'ngx-toastr';
import { error } from 'console';

@Component({
  selector: 'app-admin-page',
  standalone: true,
  imports: [NavbarComponentAdmin, CommonModule, FormsModule],
  templateUrl: './admin-page.component.html',
  styleUrl: './admin-page.component.scss'
})
export class AdminPageComponent implements OnInit {
  constructor(private adminService: AdminPageService, private toastr: ToastrService){}

  deviceCount: number = 0;
  devices : Device[] = [];

  ngOnInit() : void {
    this.adminService.getDevices().subscribe(
      (data) => {
        this.devices = [];
        this.devices = data;
      },
      (error) => {
        this.toastr.error(error.message);
      }
    )

  }

  onEnter(event: Event): void {
    const keyboardEvent = event as KeyboardEvent;
    keyboardEvent.preventDefault();  
    console.log(keyboardEvent.key);  
  }

  deleteDevice(deviceId : string) {
    this.adminService.deleteDevice(deviceId).subscribe(
      (data: any) => {
        this.devices= this.devices.filter(device => device.deviceId !== deviceId);
        this.toastr.success(data.message);
      },
      (error) => {
        this.toastr.error(error.error);
      }

    )
  }

  submitDevices() {
    this.adminService.addDevice(this.deviceCount).subscribe(
      (data) => {
        this.devices = [];
        this.devices = data;
      },
      (error) => {
        console.log(error.error);
        this.toastr.error(error.error);
      }
    )
  }



}


