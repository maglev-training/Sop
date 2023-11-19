import { ApplicationConfig } from '@angular/core';
import { provideRouter } from '@angular/router';

import { routes } from './app.routes';
import { provideState, provideStore } from '@ngrx/store';
import { reducers } from './state';
import { provideStoreDevtools } from '@ngrx/store-devtools';
import { provideHttpClient, withInterceptors } from '@angular/common/http';
import { secureApiInterceptor } from './auth/secure-api.interceptor';
import { authFeature } from './state/auth';
import { AuthEffects } from './state/auth/effects';
import { provideEffects } from '@ngrx/effects';
import { UserEffects } from './user/state/effects';
import { userFeature } from './user/state';

export const appConfig: ApplicationConfig = {
  providers: [
    provideRouter(routes),
    provideHttpClient(withInterceptors([secureApiInterceptor])),
    provideStore(reducers),
    provideState(authFeature),
    provideState(userFeature),
    provideEffects([AuthEffects, UserEffects]),
    provideStoreDevtools(),
  ],
};
