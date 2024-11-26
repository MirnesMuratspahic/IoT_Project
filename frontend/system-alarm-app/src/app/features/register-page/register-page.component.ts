import { Component, OnInit } from '@angular/core';
import {
  FormBuilder,
  FormGroup,
  Validators,
  ReactiveFormsModule,
} from '@angular/forms';
import { CommonModule } from '@angular/common';
import { Router } from '@angular/router';
import { RegisterPageService } from './services/register-page.service';
import { RegisterData } from './models/register-data.model';

@Component({
  selector: 'app-registration',
  standalone: true,
  templateUrl: './register-page.component.html',
  styleUrls: ['./register-page.component.scss'],
  imports: [ReactiveFormsModule, CommonModule],
})
export class RegisterPageComponent implements OnInit {
  registrationForm: FormGroup;
  errorMessage: string | null = null;

  constructor(
    private fb: FormBuilder,
    private router: Router,
    private registerService: RegisterPageService
  ) {
    this.registrationForm = this.fb.group({
      email: ['', [Validators.required, Validators.email]],
      password: ['', Validators.required],
      firstName: ['', Validators.required],
      lastName: ['', Validators.required],
      phoneNumber: ['', Validators.required],
      role: ['User'],
    });
  }

  ngOnInit(): void {}

  onSubmit(): void {
    if (this.registrationForm.valid) {
      const userData: RegisterData = this.registrationForm.value;

      this.registerService.register(userData).subscribe({
        next: (response) => {
          console.log(response);
          this.errorMessage = null;
          localStorage.setItem('userEmail', userData.email);
          this.router.navigate(['/code']);
        },
        error: (error) => {
          console.error('Error registering user', error);
          this.errorMessage = 'Registration failed. Please try again.';
        },
      });
    } else {
      console.log('Form not valid');
    }
  }

  onLoginPage(): void {
    this.router.navigate(['']);
  }
}
