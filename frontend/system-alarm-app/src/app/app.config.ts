import { ApplicationConfig, provideZoneChangeDetection } from '@angular/core';
import { provideToastr } from 'ngx-toastr';
import { provideRouter } from '@angular/router';
import { provideHttpClient } from '@angular/common/http';
import { withFetch } from '@angular/common/http';

import { appRoutes } from './app.routes';
import { provideClientHydration, withEventReplay } from '@angular/platform-browser';

export const appConfig: ApplicationConfig = {
  providers: [provideZoneChangeDetection({ eventCoalescing: true }), provideRouter(appRoutes), provideClientHydration(withEventReplay()), provideHttpClient(withFetch()), provideToastr({}),]
};
