export const navbarLinksAdmin = [
  {
    title: 'Logout',
    href: '/',
    onClick: () => {
      localStorage.removeItem('token');
    },
  },
];
