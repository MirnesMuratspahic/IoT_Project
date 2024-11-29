import { Component } from '@angular/core';
import {
  ReactiveFormsModule,
  FormBuilder,
  FormGroup,
  Validators,
} from '@angular/forms';
import { CommonModule } from '@angular/common';
import { Router } from '@angular/router';
import { LoginPageService } from './services/login-page.service';
import { LoginData } from './models/login-data.model';
import { error } from 'console';
import { AuthService } from '../services/auth.service';

@Component({
  selector: 'app-login',
  standalone: true,
  templateUrl: './login-page.component.html',
  styleUrls: ['./login-page.component.scss'],
  imports: [ReactiveFormsModule, CommonModule],
})
export class LoginPageComponent {
  loginForm: FormGroup;
  errorMessage: string | null = null;

  constructor(
    private fb: FormBuilder,
    private loginService: LoginPageService,
    private router: Router,
    private auth: AuthService
  ) {
    this.loginForm = this.fb.group({
      email: ['', [Validators.required, Validators.email]],
      password: ['', Validators.required],
    });
  }

  async onLoginSubmit() {
    if (this.loginForm.valid) {
      const loginData: LoginData = this.loginForm.value;

      this.loginService.login(loginData).subscribe(
        (data) => {
          localStorage.setItem('token', data);
          const role = this.auth.getRoleFromToken();
          if(role === 'User')
            this.router.navigate(['/profile']);
          else if(role === 'Admin')
            this.router.navigate(['/admin'])
        },
        (error) => {
          console.log(error);
          this.errorMessage = error.message;
        }
      )
    }
  }

  onRegisterPage() {
    this.router.navigate(['/register']);
  }
}
