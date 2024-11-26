import { Routes } from '@angular/router';
import { LoginPageComponent } from './features/login-page/login-page.component';
import { RegisterPageComponent } from './features/register-page/register-page.component';
import { ProfilePageComponent } from './features/profile-page/profile-page.component';
import { CodePageComponent } from './features/code-page/code-page.component';

export const appRoutes: Routes = [
    {path: '' , component: LoginPageComponent},
    {path: 'code', component: CodePageComponent},
    {path: 'register' , component: RegisterPageComponent},
    {path: 'profile' , component: ProfilePageComponent}
];


