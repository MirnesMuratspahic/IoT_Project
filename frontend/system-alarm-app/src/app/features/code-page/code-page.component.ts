import { Component } from '@angular/core';
import { NgForm } from '@angular/forms';
import { NgModule } from '@angular/core';
import { FormsModule } from '@angular/forms'; 
import { AppComponent } from '../../app.component';
import { CodePageService } from './service/code-page.service';
import { dtoUserCode } from './models/dtoUserCode.model';
import { AuthService } from '../services/auth.service';
import { ToastrService } from 'ngx-toastr';
import { Router } from '@angular/router';
import { error } from 'console';
import { CommonModule } from '@angular/common';


@Component({
  selector: 'app-code-page',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './code-page.component.html',
  styleUrl: './code-page.component.scss'
})
export class CodePageComponent {
  icode: string = '';
  userEmail: string = '';

  dtoUserCode : dtoUserCode = {
    email: this.userEmail,
    code: this.icode
  };

  constructor(private codePageService: CodePageService, private authService: AuthService, 
    private toastr: ToastrService, private router: Router){}


  onSubmit() {
    if (this.icode.length === 6) {
      this.userEmail = localStorage.getItem('userEmail') || '';
      this.dtoUserCode.email = this.userEmail;
      this.dtoUserCode.code = this.icode;
      console.log('Submitted icode:', this.icode);
      console.log(this.dtoUserCode);
      this.codePageService.sendCodeToVerifyEmail(this.dtoUserCode).subscribe(
        (data: string) => {
          console.log(data);
          this.router.navigate(['']);
          localStorage.removeItem('userEmail');
      },
      (error) => {
        console.log(error);
        this.toastr.error(error);
      });
    } else {
      console.error('Invalid code.');
    }
  }

  resendCode() {
    console.log('Resending the confirmation code...');
  }
}
