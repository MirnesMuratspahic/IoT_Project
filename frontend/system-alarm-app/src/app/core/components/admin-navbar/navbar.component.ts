import { Component } from '@angular/core';
import { navbarLinksAdmin } from '../../../constants/navbarLinksAdmin';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';

@Component({
  selector: 'app-navbar',
  standalone: true,
  templateUrl: './navbar.component.html',
  styleUrls: ['./navbar.component.scss'],
  imports: [CommonModule, RouterModule],
})
export class NavbarComponentAdmin {
  navbarLinks = navbarLinksAdmin;
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
