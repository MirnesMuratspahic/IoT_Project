import { Injectable, Inject, PLATFORM_ID } from '@angular/core';
import { jwtDecode } from 'jwt-decode';
import { isPlatformBrowser } from '@angular/common';

@Injectable({
  providedIn: 'root',
})
export class AuthService {
  constructor(@Inject(PLATFORM_ID) private platformId: Object) {}

  private isLocalStorageAvailable(): boolean {
    return isPlatformBrowser(this.platformId);
  }

  getEmailFromToken(): string | null {
    if (!this.isLocalStorageAvailable()) {
      return null;
    }

    const token = localStorage.getItem('token');

    if (token) {
      try {
        const decodedToken: any = jwtDecode(token);
        return decodedToken.sub; 
      } catch (error) {
        console.error('Greška prilikom dekodiranja tokena:', error);
        return null;
      }
    } else {
      console.error('Token nije pronađen u localStorage');
      return null;
    }
  }

  getRoleFromToken(): string | null {
    if (!this.isLocalStorageAvailable()) {
      return null;
    }
  
    const token = localStorage.getItem('token');
  
    if (token) {
      try {
        const decodedToken: any = jwtDecode(token);
        console.log(decodedToken.role);
        return decodedToken.role || 
               decodedToken['http://schemas.microsoft.com/ws/2008/06/identity/claims/role'] || 
               null;
      } catch (error) {
        console.error('Greška prilikom dekodiranja tokena:', error);
        return null;
      }
    } else {
      console.error('Token nije pronađen u localStorage');
      return null;
    }
  }
  
}
