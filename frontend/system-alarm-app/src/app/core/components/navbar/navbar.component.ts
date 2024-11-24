import { Component } from '@angular/core';
import { navbarLinks } from '../../../constants/navbarLinks';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';

@Component({
  selector: 'app-navbar',
  standalone: true,
  templateUrl: './navbar.component.html',
  styleUrls: ['./navbar.component.scss'],
  imports: [CommonModule, RouterModule],
})
export class NavbarComponent {
  navbarLinks = navbarLinks;
  isNavbarCollapsed = true;

  toggleNavbar() {
    this.isNavbarCollapsed = !this.isNavbarCollapsed;
  }

  handleLinkClick(link: any) {
    if (link.onClick) {
      link.onClick();
    }
  }
}
