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
    private router: Router
  ) {
    this.loginForm = this.fb.group({
      email: ['', [Validators.required, Validators.email]],
      password: ['', Validators.required],
    });
  }

  async onLoginSubmit() {
    if (this.loginForm.valid) {
      const loginData: LoginData = this.loginForm.value;

      try {
        const response = await this.loginService.login(loginData).toPromise();
        if (response) {
          console.log('Login successful', response);
          localStorage.setItem('token', response.token);
          this.errorMessage = null;
          this.router.navigate(['/profile']);
        }
      } catch (error: any) {
        this.errorMessage = error.message;
      }
    }
  }

  onRegisterPage() {
    this.router.navigate(['/register']);
  }
}
