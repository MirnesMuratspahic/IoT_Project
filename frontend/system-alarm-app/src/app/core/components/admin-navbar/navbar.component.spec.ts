import { ComponentFixture, TestBed } from '@angular/core/testing';
import { NavbarComponentAdmin } from './navbar.component';

describe('NavbarComponent', () => {
  let component: NavbarComponentAdmin;
  let fixture: ComponentFixture<NavbarComponentAdmin>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [NavbarComponentAdmin]
    })
    .compileComponents();

    fixture = TestBed.createComponent(NavbarComponentAdmin);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
