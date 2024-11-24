export const navbarLinks = [
  {
    title: 'About',
    href: '/about',
  },
  {
    title: 'Profile',
    href: '/profile',
  },
  {
    title: 'Logout',
    href: '/',
    onClick: () => {
      localStorage.removeItem('token');
    },
  },
];
